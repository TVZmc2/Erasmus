using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Panorama;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace ErasmusAppTVZ
{
    public partial class CityOptionsPanorama : PhoneApplicationPage
    {
        private PanoramaModel panoramaData;

        public CityOptionsPanorama()
        {
            InitializeComponent();

            panoramaData = new PanoramaModel();

            DataContext = panoramaData;

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string cityName = NavigationContext.QueryString["cityName"];
            MainPanorama.Title = cityName;
        }

        /// <summary>
        /// Builds a localized application bar with icons and menu items
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarMenuItem profileMenuItem = new ApplicationBarMenuItem();
            ApplicationBarMenuItem optionsMenuItem = new ApplicationBarMenuItem();
            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem();

            profileMenuItem.Text = AppResources.ApplicationBarProfileMenuItem;
            optionsMenuItem.Text = AppResources.ApplicationBarOptionsMenuItem;
            aboutMenuItem.Text = AppResources.ApplicationBarAboutMenuItem;

            aboutMenuItem.Click += aboutMenuItem_Click;

            ApplicationBar.MenuItems.Add(profileMenuItem);
            ApplicationBar.MenuItems.Add(optionsMenuItem);
            ApplicationBar.MenuItems.Add(aboutMenuItem);

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.IsVisible = true;
        }

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            Grid grid = ApplicationBarHelper.GetAboutContentGrid();

            CustomMessageBox aboutMsgBox = new CustomMessageBox() 
            {
                Caption = AppResources.ApplicationBarAboutMenuItem,
                Content = grid,
                RightButtonContent = "ok"
            };

            aboutMsgBox.Show();
        }
        #endregion

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

    }//class
}//namespace