using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Country;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Media.Imaging;
using System.IO;
using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.ViewModel.University;
using ErasmusAppTVZ.ViewModel.Programme;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace ErasmusAppTVZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        //public List<string> Values;
        //public List<string> UniversityNames;
        //public List<string> ProgrammeNames { get; set; }

        public CountryModel Country;
        public UniversityModel University;
        public List<int> univIndex;

        public static bool isFirstNavigation = true;
        private int selectedCountryIndex;

        public int loginCountryID;
        public string loginProgrammeCategory;

        //public IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //if ((App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush).Color == Colors.White)
            //    listPickerCountries.Background = new SolidColorBrush(Colors.Gray);

            InitializeStudProf();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Initializes content for listPickerStudProf element
        /// </summary>
        private void InitializeStudProf()
        {
            listPickerStudProf.Items.Add("Student");
            listPickerStudProf.Items.Add("Professor");
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

                //UniversityNames = await App.MobileService.GetTable<UniversityData>().Where(x => x.CountryId == 1).Select(x => x.Name).ToListAsync();

                //listPickerUniversities.ItemsSource = UniversityNames;

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
        //private void listPickerStudProf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (listPickerStudProf.SelectedItem.ToString() == "Professor")
        //    {
        //        listPickerPrograms.IsEnabled = false;
        //        listPickerPrograms.ItemsSource = null;
        //    }
        //    else
        //    {
        //        listPickerPrograms.IsEnabled = true;
        //        listPickerUniversities_SelectionChanged(sender, null);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (listPickerCountries.SelectedItem != null)
            //{
            //    List<int> tempCountry = await App.MobileService.GetTable<CountryData>().
            //        Where(x => x.Name == (listPickerCountries.SelectedItem as CountryData).Name).
            //        Select(x => x.Id).ToListAsync();

            //    loginCountryID = tempCountry.First();
            //}

            IsolatedStorageSettings.ApplicationSettings["selectedCountryIndex"] = selectedCountryIndex;
            IsolatedStorageSettings.ApplicationSettings.Save();

        
            //if (listPickerPrograms.SelectedItem != null)
            //{
            //    List<string> tempCategory = await App.MobileService.GetTable<ProgrammeData>().
            //        Where(x => x.Name == listPickerPrograms.SelectedItem.ToString()).
            //        Select(x => x.Category).ToListAsync();

            //    loginProgrammeCategory = tempCategory.First();
            //}

            //if (isf.FileExists("login.txt"))
            //{
            //    isf.DeleteFile("login.txt");
            //}

            //using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("login.txt", FileMode.CreateNew, isf))
            //{
            //    using (StreamWriter writer = new StreamWriter(isoStream))
            //    {
            //        writer.WriteLine(listPickerCountries.SelectedIndex.ToString());
            //    }
            //}

            CheckBox checkBox = new CheckBox()
            {
                Content = AppResources.MessageBoxRemember,
                Margin = new Thickness(0, 12, 0, 0)
            };

            CustomMessageBox rememberMeMsgBox = new CustomMessageBox()
            {
                Caption = AppResources.MessageBoxCaption,
                Message = "If you choose to save your preferences, you will no longer see this screen. " +
                "But, if you want to change them later, you can easily access preference options located in Application Bar. ",
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
                            string[] preferences = new string[3];
                            preferences[0] = selectedCountryIndex.ToString();
                            preferences[1] = listPickerStudProf.SelectedIndex.ToString();
                            preferences[2] = listPickerPrograms.SelectedIndex.ToString();

                            //serialize for easier saving
                            string result = JsonConvert.SerializeObject(preferences);

                            //save under 'preferences' key
                            IsolatedStorageSettings.ApplicationSettings["preferences"] = result;
                            IsolatedStorageSettings.ApplicationSettings.Save();
                        }

                        NavigationService.Navigate(new Uri("/CountrySelect.xaml", UriKind.Relative));
                        break;
                    //case CustomMessageBoxResult.None:
                    //    break;
                    //case CustomMessageBoxResult.RightButton:
                    //    break;
                    default:
                        break;
                }
            };

            rememberMeMsgBox.Show();
        }

        /// <summary>
        /// Probably not needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    CountryData flag = (sender as ListPicker).SelectedItem as CountryData;

        //    if (flag != null)
        //        img.Source = flag.FlagImage;
        //}

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

            univIndex = null;
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

                univIndex = await App.MobileService.GetTable<UniversityData>().
                    Where(x => x.Name == listPickerUniversities.SelectedItem.ToString()).
                    Select(x => x.ID).ToListAsync();

                List<string> ProgrammeNames = await App.MobileService.GetTable<ProgrammeData>().
                    Where(x => x.UniversityId == univIndex.First()).
                    Select(x => x.Name).ToListAsync();

                listPickerPrograms.ItemsSource = ProgrammeNames;

                ProgressIndicatorHelper.SetProgressBar(false, null);
            }
        }


        #endregion
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}