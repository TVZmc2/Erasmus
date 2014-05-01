using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Country;
using ErasmusAppTVZ.ViewModel.Programme;
using ErasmusAppTVZ.ViewModel.University;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        public CountryModel Country;
        public UniversityModel University;

        private static bool isFirstNavigation = true;
        private int selectedCountryIndex;
        private string role;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if user preferences already exist.
        /// If so, redirect user to the next page.
        /// If not, initialize starting page
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isFirstNavigation)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("preferences"))
                {
                    string[] preferences = JsonConvert.DeserializeObject<string[]>
                        (IsolatedStorageSettings.ApplicationSettings["preferences"].ToString());

                    NavigationService.Navigate(new Uri(string.Format("/CountrySelect.xaml?countryId={0}&role={1}&prog={2}",
                        preferences[0], preferences[1], preferences[2]), UriKind.Relative));
                    return;
                }

                Country = new CountryModel()
                {
                    Countries = await App.MobileService.GetTable<CountryData>().ToListAsync()
                };

                foreach (CountryData data in Country.Countries)
                {
                    data.FlagImage = ImageConversionHelper.ToImage(data.Flag);
                    data.Flag = String.Empty;
                }

                DataContext = Country;

                isFirstNavigation = false;
            }
        }

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["selectedCountryIndex"] = selectedCountryIndex;
            IsolatedStorageSettings.ApplicationSettings.Save();

            CheckBox checkBox = new CheckBox()
            {
                Content = AppResources.MessageBoxRemember,
                Margin = new Thickness(0, 12, 0, 0)
            };

            CustomMessageBox rememberMeMsgBox = new CustomMessageBox()
            {
                Caption = AppResources.MessageBoxRememberCaption,
                Message = AppResources.MessageBoxRememberText,
                Content = checkBox,
                LeftButtonContent = "ok",
            };

            //Prompts the user if he wants his choices remembered or not
            rememberMeMsgBox.Dismissed += (sender1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        if ((bool)checkBox.IsChecked)
                        {
                            //Remember user preferences
                            string[] preferences = new string[4];
                            preferences[0] = selectedCountryIndex.ToString();
                            preferences[1] = role;
                            preferences[2] = listPickerUniversities.SelectedItem.ToString();
                            preferences[3] = listPickerPrograms.SelectedIndex.ToString();

                            //serialize for easier saving
                            string result = JsonConvert.SerializeObject(preferences);

                            //save under 'preferences' key
                            IsolatedStorageSettings.ApplicationSettings["preferences"] = result;
                            IsolatedStorageSettings.ApplicationSettings.Save();
                        }

                        NavigationService.Navigate(new Uri("/CountrySelect.xaml", UriKind.Relative));
                        break;
                    default:
                        break;
                }
            };

            rememberMeMsgBox.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listPickerCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorUniversities);

            selectedCountryIndex = (sender as ListPicker).SelectedIndex + 1;

            listPickerPrograms.ItemsSource = null;

            List<string> UniversityNames = await App.MobileService.GetTable<UniversityData>().
                Where(x => x.CountryId == selectedCountryIndex).
                Select(x => x.Name).ToListAsync();

            listPickerUniversities.ItemsSource = UniversityNames;

            ProgressIndicatorHelper.SetProgressBar(false, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listPickerUniversities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listPickerUniversities.SelectedItem != null)
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();

                ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorPrograms);

                List<int> univIndex = await App.MobileService.GetTable<UniversityData>().
                    Where(x => x.Name == listPickerUniversities.SelectedItem.ToString()).
                    Select(x => x.ID).ToListAsync();

                listPickerPrograms.ItemsSource = await App.MobileService.GetTable<ProgrammeData>().
                    Where(x => x.UniversityId == univIndex.First()).Select(x => x.Name).ToListAsync();

                ProgressIndicatorHelper.SetProgressBar(false, null);
            }
        }

        /// <summary>
        /// Changes role to professor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonProfessor_Click(object sender, RoutedEventArgs e)
        {
            role = "professor";
        }

        /// <summary>
        /// Changes role to student
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            role = "student";
        }

        #endregion
    }
}