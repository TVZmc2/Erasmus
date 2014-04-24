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

namespace ErasmusAppTVZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/CountrySelect.xaml", UriKind.Relative));
        }

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