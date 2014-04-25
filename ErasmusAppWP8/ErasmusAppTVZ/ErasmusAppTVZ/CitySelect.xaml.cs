using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.City;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class CitySelect : PhoneApplicationPage
    {
        private CityModel cm;

        public CitySelect()
        {
            InitializeComponent();

            cm = new CityModel();
            cm.LoadData();

            DataContext = cm;

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Sets the visibillity and indertermination of ProgressIndicator
        /// </summary>
        /// <param name="check"></param>
        private void SetProgressBar(bool check)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = check;
            SystemTray.ProgressIndicator.IsVisible = check;
        }

        /// <summary>
        /// Checks if 'search' and 'countryId' parameters exist
        /// If 'search' exists, get the filtered results and pass it to DataContext
        /// If 'countryId' exists, get the data for the corresponding country
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            textBoxSearch.Text = String.Empty;
            textBoxSearch.Visibility = System.Windows.Visibility.Collapsed;

            if (NavigationContext.QueryString.ContainsKey("search"))
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SetProgressBar(true);

                string searchTerm = NavigationContext.QueryString["search"];

                CityModel model = new CityModel()
                {
                    Cities = cm.Cities.Where(x => x.Name.Contains(searchTerm)).ToList()
                };

                DataContext = model;

                SetProgressBar(false);
            }
            else if (NavigationContext.QueryString.ContainsKey("countryId"))
            {
                //get id for retrieving country data
                int id = 0;
                Int32.TryParse(NavigationContext.QueryString["countryId"], out id);

                //TODO:
                //Get the data based on id (countryId)
            }
        }

        /// <summary>
        /// Build a localized application bar with icons and menu items
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton searchIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton showOnMapIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton sortIconButton = new ApplicationBarIconButton();

            searchIconButton.Text = AppResources.ApplicationBarSearch;
            showOnMapIconButton.Text = AppResources.ApplicationBarHideMap;
            sortIconButton.Text = AppResources.ApplicationBarSort;

            searchIconButton.IconUri = new Uri("/Assets/AppBar/search.png", UriKind.Relative);
            showOnMapIconButton.IconUri = new Uri("/Assets/AppBar/map.png", UriKind.Relative);
            sortIconButton.IconUri = new Uri("/Assets/AppBar/sort.png", UriKind.Relative);

            searchIconButton.Click += searchIconButton_Click;
            showOnMapIconButton.Click += showOnMapIconButton_Click;
            sortIconButton.Click += sortIconButton_Click;

            ApplicationBar.Buttons.Add(searchIconButton);
            ApplicationBar.Buttons.Add(showOnMapIconButton);
            ApplicationBar.Buttons.Add(sortIconButton);
        }

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortIconButton_Click(object sender, EventArgs e)
        {
            //TODO:
            //implement sorting
            MessageBox.Show("Not implemented");
        }

        /// <summary>
        /// Show or hide map and change the text of IconButton appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showOnMapIconButton_Click(object sender, EventArgs e)
        {
            if (map.Visibility == System.Windows.Visibility.Visible)
            {
                map.Visibility = System.Windows.Visibility.Collapsed;
                (sender as ApplicationBarIconButton).Text = AppResources.ApplicationBarShowMap;
                return;
            }

            (sender as ApplicationBarIconButton).Text = AppResources.ApplicationBarHideMap;
            map.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Check if textBoxSearch.Text.Length is greater than 0
        /// If yes, than refresh page with search parameter
        /// If not, show TextBox and get focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchIconButton_Click(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length == 0)
            {
                textBoxSearch.Visibility = System.Windows.Visibility.Visible;
                textBoxSearch.Focus();
            }
            else
                NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml" +
                                    "?Refresh=true&search={0}", textBoxSearch.Text), UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;

            NavigationService.Navigate(new Uri(string.Format("/CityOptionsPanorama.xaml?cityName={0}", bttn.Tag.ToString().ToLower()),
                UriKind.Relative));
        } 
        #endregion
    
    }//class
}//namespace