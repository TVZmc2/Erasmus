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
        /// Build a localized application bar with icons and menu items
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
            XElement appInfo = XDocument.Load("WMAppManifest.xml")
                .Root.Element("App");

            string title = appInfo.Attribute("Title").Value;
            string version = appInfo.Attribute("Version").Value;
            string description = appInfo.Attribute("Description").Value;

            Grid grid = new Grid() { Margin = new Thickness(12, 0, 0, 0) };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            TextBlock txtBlockTitle = new TextBlock() 
            {
                Text = title,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeLarge"].ToString()) 
            };

            TextBlock txtBlockMessage = new TextBlock() 
            {
                Text = string.Format("\n{0}\nVersion: {1}",
                    description, version),
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
            };

            Grid.SetRow(txtBlockMessage, 1);
            grid.Children.Add(txtBlockTitle);
            grid.Children.Add(txtBlockMessage);

            CustomMessageBox aboutMsgBox = new CustomMessageBox() 
            {
                Caption = AppResources.ApplicationBarAboutMenuItem,
                Content = grid,
                RightButtonContent = "ok"
            };

            aboutMsgBox.Show();
        }
        #endregion

    }//class
}//namespace