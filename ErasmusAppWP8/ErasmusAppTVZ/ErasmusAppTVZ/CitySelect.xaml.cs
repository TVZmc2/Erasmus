using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.City;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class CitySelect : PhoneApplicationPage
    {
        //constants for map zoom level
        private const double ZOOM_LEVEL_COUNTRY = 5;
        private const double ZOOM_LEVEL_CITY = 12;

        //helper for deciding which sort parameter is used
        private static int sortCounter = 0;

        //helpers for preserving and controlling elements state
        private static bool isExpanderTapped;
        private static bool isMapVisible;

        //arrays for storing latitude and longitude
        private double[] countryCoordinates;
        private static double[] cityCoordinates;

        private static CityModel model;

        /// <summary>
        /// Constructor
        /// </summary>
        public CitySelect()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Checks if 'search' and 'countryId' parameters exist
        /// If 'search' exists, get the filtered results
        /// If 'countryId' exists, get the data for the corresponding country
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            listbox.Opacity = 0;

            textBoxSearch.Text = String.Empty;
            textBoxSearch.Visibility = System.Windows.Visibility.Collapsed;

            if (NavigationContext.QueryString.ContainsKey("search"))
            {
                sortCounter = 0;

                string searchTerm = NavigationContext.QueryString["search"];

                CityModel cm = new CityModel()
                {
                    Cities = model.Cities.Where(x => x.Name.Contains(searchTerm)).ToList()
                };

                DataContext = cm;

                if (isMapVisible)
                {
                    map.Visibility = System.Windows.Visibility.Visible;
                    CoordinatesHelper.SetMapCenter(ref map, cityCoordinates, ZOOM_LEVEL_CITY);
                }
            }
            else if (NavigationContext.QueryString.ContainsKey("countryId"))
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorCities);

                //get id for retrieving country data
                int id = 0;
                Int32.TryParse(NavigationContext.QueryString["countryId"], out id);

                isMapVisible = false;
                bool.TryParse(NavigationContext.QueryString["mapVisible"], out isMapVisible);

                cityCoordinates = new double[2];
                countryCoordinates = new double[2];
                double.TryParse(NavigationContext.QueryString["lat"], out countryCoordinates[0]);
                double.TryParse(NavigationContext.QueryString["lon"], out countryCoordinates[1]);

                if (isMapVisible)
                {
                    CoordinatesHelper.SetMapCenter(ref map, countryCoordinates, ZOOM_LEVEL_COUNTRY);
                    map.Visibility = System.Windows.Visibility.Visible;
                }

                //test data
                model = new CityModel();
                model.LoadData();

                DataContext = model;

                ProgressIndicatorHelper.SetProgressBar(false, null);

                //TODO:
                //Get the data based on id (countryId)
            }

            AnimationHelper.Fade(listbox, 1, 750, new PropertyPath(OpacityProperty));
        }

        /// <summary>
        /// Build a localized application bar with icons and menu items
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            //Icon buttons
            ApplicationBarIconButton searchIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton showMapIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton sortIconButton = new ApplicationBarIconButton();

            searchIconButton.Text = AppResources.ApplicationBarSearch;
            showMapIconButton.Text = AppResources.ApplicationBarShowMap;
            sortIconButton.Text = AppResources.ApplicationBarSort;

            searchIconButton.IconUri = new Uri("/Assets/AppBar/search.png", UriKind.Relative);
            showMapIconButton.IconUri = new Uri("/Assets/AppBar/map.png", UriKind.Relative);
            sortIconButton.IconUri = new Uri("/Assets/AppBar/sort.png", UriKind.Relative);

            searchIconButton.Click += searchIconButton_Click;
            showMapIconButton.Click += showMapIconButton_Click;
            sortIconButton.Click += sortIconButton_Click;

            //Menu items
            ApplicationBarMenuItem profileMenuItem = new ApplicationBarMenuItem();
            ApplicationBarMenuItem optionsMenuItem = new ApplicationBarMenuItem();
            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem();

            profileMenuItem.Text = AppResources.ApplicationBarProfileMenuItem;
            optionsMenuItem.Text = AppResources.ApplicationBarOptionsMenuItem;
            aboutMenuItem.Text = AppResources.ApplicationBarAboutMenuItem;

            profileMenuItem.Click += profileMenuItem_Click;
            optionsMenuItem.Click += optionsMenuItem_Click;
            aboutMenuItem.Click += aboutMenuItem_Click;

            ApplicationBar.Buttons.Add(searchIconButton);
            ApplicationBar.Buttons.Add(showMapIconButton);
            ApplicationBar.Buttons.Add(sortIconButton);

            ApplicationBar.MenuItems.Add(profileMenuItem);
            ApplicationBar.MenuItems.Add(optionsMenuItem);
            ApplicationBar.MenuItems.Add(aboutMenuItem);

            ApplicationBar.IsVisible = true;
        }

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void optionsMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void profileMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExpanderView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!isExpanderTapped)
                isExpanderTapped = true;

            ExpanderView ev = sender as ExpanderView;

            if (ev.IsExpanded)
            {
                //hardcoded Zagreb for testing purposes
                cityCoordinates = await CoordinatesHelper.GetCoordinates("Zagreb", 2);

                CoordinatesHelper.SetMapCenter(ref map, cityCoordinates, ZOOM_LEVEL_CITY);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderView_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;

            NavigationService.Navigate(new Uri(string.Format("/CityOptionsPanorama.xaml?cityName={0}", ev.Tag.ToString().ToLower()),
                UriKind.Relative));
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortIconButton_Click(object sender, EventArgs e)
        {
            if (sortCounter == 0)
                DataContext = new CityModel() { Cities = (DataContext as CityModel).Cities.OrderByDescending(x => x.Rating).ToList() };
            else if (sortCounter == 1)
                DataContext = new CityModel() { Cities = (DataContext as CityModel).Cities.OrderByDescending(x => x.Name).ToList() };
            else
                DataContext = new CityModel() { Cities = (DataContext as CityModel).Cities.OrderBy(x => x.Name).ToList() };

            sortCounter += 1;

            if (sortCounter == 3)
                sortCounter = 0;
        }

        /// <summary>
        /// Show or hide map and change the text of IconButton appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showMapIconButton_Click(object sender, EventArgs e)
        {
            if (map.Visibility == System.Windows.Visibility.Visible)
            {
                map.Visibility = System.Windows.Visibility.Collapsed;
                (sender as ApplicationBarIconButton).Text = AppResources.ApplicationBarShowMap;
                isMapVisible = false;
                return;
            }

            (sender as ApplicationBarIconButton).Text = AppResources.ApplicationBarHideMap;
            map.Visibility = System.Windows.Visibility.Visible;
            isMapVisible = true;

            if(isExpanderTapped)
                CoordinatesHelper.SetMapCenter(ref map, cityCoordinates, ZOOM_LEVEL_CITY);
            else
                CoordinatesHelper.SetMapCenter(ref map, countryCoordinates, ZOOM_LEVEL_COUNTRY);
            
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;

            NavigationService.Navigate(new Uri(string.Format("/CityOptionsPanorama.xaml?cityName={0}", bttn.Tag.ToString().ToLower()),
                UriKind.Relative));
        }
        #endregion

    }//class
}//namespace