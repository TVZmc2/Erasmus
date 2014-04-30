using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Interest;
using ErasmusAppTVZ.ViewModel.Language;
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

            profileMenuItem.Click += profileMenuItem_Click;
            aboutMenuItem.Click += aboutMenuItem_Click;

            ApplicationBar.MenuItems.Add(profileMenuItem);
            ApplicationBar.MenuItems.Add(optionsMenuItem);
            ApplicationBar.MenuItems.Add(aboutMenuItem);

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.IsVisible = true;
        }

        async void profileMenuItem_Click(object sender, EventArgs e)
        {
            List<InterestData> Interests = await App.MobileService.GetTable<InterestData>().ToListAsync();
            List<LanguageData> Languages = await App.MobileService.GetTable<LanguageData>().ToListAsync();

            CustomMessageBox customMessageBox = ContentHelper.GetStudentProfileEditor(true, Interests, Languages);

            customMessageBox.Show();
        }

        #region EventHandlers
        /// <summary>
        /// Gets the CustomMessageBox with content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            CustomMessageBox aboutMessageBox = ContentHelper.GetAboutMessageBox();
            aboutMessageBox.Show();
        }
        #endregion

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridStudents_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //test data and formatting
            string content = "17.03.1992,London,Great Britain,Cambridge;" +
                             "English,German,Spanish,Danish,Croatian,Italian,Slovenian;" +
                             "Science fiction,Astronomy,Sports,Physics,Mathematics,Cars,Books,Cooking;" +
                             "someGuy,blabla;";

            CustomMessageBox customMessageBox = ContentHelper.GetStudentProfileViewer(
                (sender as Grid).Tag.ToString(), content);

            customMessageBox.Show();
        }

    }//class
}//namespace