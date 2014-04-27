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
using System.Windows.Media;
using ErasmusAppTVZ.ViewModel.University;
using ErasmusAppTVZ.ViewModel.Programme;

namespace ErasmusAppTVZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        public CountryModel Country;
        public UniversityModel University;

        public List<string> FlagValues;
        public List<string> UniversityNames;
        public List<string> ProgrammeNames;
        public List<int> univIndex;

        public static bool isFirstNavigation = true;
        private int selectedCountryIndex;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;

            InitializeStudProf();

            // ((SolidColorBrush)App.Current.Resources["PhoneChromeBrush"]).Color = Colors.Black;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        public void InitializeStudProf()
        {
            listPickerStudProf.Items.Add("Student");
            listPickerStudProf.Items.Add("Professor");
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isFirstNavigation)
            {
                #region Country
                Country = new CountryModel();
                FlagValues = await App.MobileService.GetTable<CountryData>().Select(x => x.Flag).ToListAsync();
                Country.Countries = new List<CountryData>();

                foreach (string s in FlagValues)
                {
                    CountryData countryData = new CountryData() 
                    { 
                        FlagImage = ImageConversionHelper.ToImage(s) 
                    };

                    Country.Countries.Add(countryData);
                }

                DataContext = Country;
                #endregion

                isFirstNavigation = false;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/CountrySelect.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = new CheckBox()
            {
                Content = "Remember me",
                Margin = new Thickness(0, 12, 0, 0)
            };

            CustomMessageBox rememberMeMsgBox = new CustomMessageBox()
            {
                Caption = "Do you want your preferences to be remembered?",
                Message = "If you choose to save your preferences, you will no longer see this screen. " +
                "But, if you want to change them later, you can easily access preference options located in Application Bar. ",
                Content = checkBox,
                LeftButtonContent = "ok",
            };

            //TODO: comment
            rememberMeMsgBox.Dismissed += (sender1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            if ((bool)checkBox.IsChecked)
                            {
                                //Remember user
                            }

                            NavigationService.Navigate(new Uri("/CountrySelect.xaml", UriKind.Relative));
                            break;
                        case CustomMessageBoxResult.None:
                            break;
                        case CustomMessageBoxResult.RightButton:
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
        async private void listPickerCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountryData flag = (sender as ListPicker).SelectedItem as CountryData;
            selectedCountryIndex = ((sender as ListPicker).SelectedIndex + 1);
            univIndex = null;
            listPickrProgramee.ItemsSource = null;

            UniversityNames = await App.MobileService.GetTable<UniversityData>().Where(x => x.CountryID == selectedCountryIndex).Select(x => x.Name).ToListAsync();
            listPickrUniversities.ItemsSource = UniversityNames;
        }

        async private void listPickrUniversities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listPickrUniversities.SelectedItem != null)
            {
                univIndex = await App.MobileService.GetTable<UniversityData>().Where(x => x.Name == listPickrUniversities.SelectedItem.ToString()).Select(x => x.ID).ToListAsync();
                ProgrammeNames = await App.MobileService.GetTable<ProgrammeData>().Where(x => x.facultyID == univIndex.First()).Select(x => x.Name).ToListAsync();
                listPickrProgramee.ItemsSource = ProgrammeNames;
            }
        }




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