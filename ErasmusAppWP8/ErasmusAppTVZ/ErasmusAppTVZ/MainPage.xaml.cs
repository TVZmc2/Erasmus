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

namespace ErasmusAppTVZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        public CountryModel Country;
        public List<string> Values;
        public static bool isFirstNavigation = true;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isFirstNavigation)
            {
                Country = new CountryModel();
                Country.Countries = await App.MobileService.GetTable<CountryData>().ToListAsync();
                //Country.Countries = new List<CountryData>();

                foreach (CountryData data in Country.Countries)
                {
                    data.FlagImage = ImageConversionHelper.ToImage(data.Flag);
                    //CountryData countryData = new CountryData() 
                    //{ 
                    //    FlagImage = ImageConversionHelper.ToImage(s) 
                    //};

                    //Country.Countries.Add(countryData);
                }

                DataContext = Country;

                isFirstNavigation = false;
            }
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
        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountryData flag = (sender as ListPicker).SelectedItem as CountryData;

            if (flag != null)
                img.Source = flag.FlagImage;
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