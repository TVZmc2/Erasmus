﻿using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.City;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class CitySelect : PhoneApplicationPage
    {
        //constants for map zoom level
        private const double ZOOM_LEVEL_COUNTRY = 5.5;
        private const double ZOOM_LEVEL_CITY = 8.75;

        //helper for deciding which sort parameter is used
        private static int sortCounter = 0;

        //helpers for preserving and controlling elements state
        private static bool hasCoordinates = false;
        private static bool isMapVisible;
        private int currentlyOpenedExpander = 0;

        //array for storing latitude and longitude
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
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
                    SetMapCenter(true);
                }
            }
            else if (NavigationContext.QueryString.ContainsKey("countryId"))
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorCities);

                //get id for retrieving country data
                int selectedCountryId = Int32.Parse(NavigationContext.QueryString["countryId"]);

                //get the value which will help to determine if map was visible or not
                isMapVisible = bool.Parse(NavigationContext.QueryString["mapVisible"]);

                cityCoordinates = new double[2];
                //countryCoordinates = new double[2];
                //double.TryParse(NavigationContext.QueryString["lat"], out countryCoordinates[0]);
                //double.TryParse(NavigationContext.QueryString["lon"], out countryCoordinates[1]);

                //Get the CityData based on selectedCountryId
                model = new CityModel() 
                {
                    Cities = await App.MobileService.GetTable<CityData>().
                        Where(x => x.CountryId == selectedCountryId).ToListAsync()
                };

                //find index of city with highest rating
                int index = 0;
                float temp = model.Cities[index].Rating;
                for (int i = 1; i < model.Cities.Count; i++)
                {
                    if (model.Cities[i].Rating > temp)
                    {
                        temp = model.Cities[i].Rating;
                        index = i;
                    }
                }

                //find geocoordinates of the city with highest rating
                FindGeoCoordinates(model.Cities[index].Name);

                //if map was visible, set it visible and center it to selected country coordinates
                if (isMapVisible)
                    map.Visibility = System.Windows.Visibility.Visible;

                DataContext = model;

                ProgressIndicatorHelper.SetProgressBar(false, null);
            }

            AnimationHelper.Fade(listbox, 1, 700, new PropertyPath(OpacityProperty));
        }

        /// <summary>
        /// Builds a localized application bar with icons and menu items
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            //Icon buttons
            ApplicationBarIconButton showMapIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton sortIconButton = new ApplicationBarIconButton();
            ApplicationBarIconButton optionsIconButton = new ApplicationBarIconButton();

            showMapIconButton.Text = AppResources.ApplicationBarShowMap;
            sortIconButton.Text = AppResources.ApplicationBarSort;
            optionsIconButton.Text = AppResources.ApplicationBarOptions;

            showMapIconButton.IconUri = new Uri("/Assets/AppBar/map.png", UriKind.Relative);
            sortIconButton.IconUri = new Uri("/Assets/AppBar/sort.png", UriKind.Relative);
            optionsIconButton.IconUri = new Uri("/Assets/AppBar/options.png", UriKind.Relative);

            showMapIconButton.Click += showMapIconButton_Click;
            sortIconButton.Click += sortIconButton_Click;
            optionsIconButton.Click += optionsIconButton_Click;

            //Menu items
            ApplicationBarMenuItem profileMenuItem = new ApplicationBarMenuItem();
            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem();

            profileMenuItem.Text = AppResources.ApplicationBarProfileMenuItem;
            aboutMenuItem.Text = AppResources.ApplicationBarAboutMenuItem;

            profileMenuItem.Click += profileMenuItem_Click;
            aboutMenuItem.Click += aboutMenuItem_Click;

            ApplicationBar.Buttons.Add(showMapIconButton);
            ApplicationBar.Buttons.Add(sortIconButton);
            ApplicationBar.Buttons.Add(optionsIconButton);

            ApplicationBar.MenuItems.Add(profileMenuItem);
            ApplicationBar.MenuItems.Add(aboutMenuItem);

            ApplicationBar.IsVisible = true;
        }



        /// <summary>
        /// Sets map center and zoom level
        /// </summary>
        private void SetMapCenter(bool CityOrCountry)
        {
            map.Center = new GeoCoordinate(cityCoordinates[0], cityCoordinates[1]);
            map.ZoomLevel = ZOOM_LEVEL_CITY;
        }

        /// <summary>
        /// Finds geocoordinates based on the search term
        /// </summary>
        /// <param name="searchTerm"></param>
        private void FindGeoCoordinates(string searchTerm)
        {
            GeocodeQuery query = new GeocodeQuery()
            {
                GeoCoordinate = new System.Device.Location.GeoCoordinate(0, 0),
                SearchTerm = searchTerm
            };

            query.QueryCompleted += query_QueryCompleted;
            query.QueryAsync();
        }

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsIconButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

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
        private void ExpanderView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;
            int evTag = Int32.Parse(ev.Tag.ToString());

            if (currentlyOpenedExpander != evTag)
            {
                hasCoordinates = false;
                currentlyOpenedExpander = evTag;

                string searchTerm = (DataContext as CityModel).Cities.Where(x => x.ID == currentlyOpenedExpander).First().Name;
                FindGeoCoordinates(searchTerm);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void query_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            //I said, trust NO ONE
            if (e.Result.Count > 0)
            {
                cityCoordinates[0] = e.Result[0].GeoCoordinate.Latitude;
                cityCoordinates[1] = e.Result[0].GeoCoordinate.Longitude;

                if (map.Visibility == System.Windows.Visibility.Visible)
                    SetMapCenter(true);

                hasCoordinates = true;
            }
        }

        /// <summary>
        /// Shortcut for selecting city
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderView_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;

            NavigationService.Navigate(new Uri(string.Format("/CityOptionsPanorama.xaml?cityId={0}", ev.Tag.ToString().ToLower()),
                UriKind.Relative));
        }

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

        /// <summary>
        /// Sorts  the city list by highest rating, and alphabetically (both ascending and descending)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortIconButton_Click(object sender, EventArgs e)
        {
            if (sortCounter == 0)
                DataContext = new CityModel() { Cities = (DataContext as CityModel).
                    Cities.OrderByDescending(x => x.Rating).ToList() };
            else if (sortCounter == 1)
                DataContext = new CityModel() { Cities = (DataContext as CityModel).
                    Cities.OrderByDescending(x => x.Name).ToList() };
            else
                DataContext = new CityModel() { Cities = (DataContext as CityModel).
                    Cities.OrderBy(x => x.Name).ToList() };

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

            if (hasCoordinates)
                SetMapCenter(true);
            else
                SetMapCenter(false);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;

            NavigationService.Navigate(new Uri(string.Format("/CityOptionsPanorama.xaml?cityId={0}", bttn.Tag.ToString().ToLower()),
                UriKind.Relative));
        }
        #endregion

    }//class
}//namespace