using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Country;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class CountrySelect : PhoneApplicationPage
    {
        //constant for map zoom level
        private const double ZOOM_LEVEL = 5.5;

        //helpers for preserving and controlling elements state
        private static bool hasCoordinates = false;
        private static bool isFirstNavigation = true;
        private static bool isMapVisible = false;
        private static string currentlyOpenedExpander = null;

        //helper for deciding which sort parameter is used
        private static int sortCounter = 0;

        //array for storing latitude and longitude
        private double[] countryCoordinates;

        private static CountryModel model;

        /// <summary>
        /// Constructor
        /// </summary>
        public CountrySelect()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("preferences"))
                Application.Current.Terminate();
        }

        /// <summary>
        /// Checks if 'search' parameter exists
        /// If parameter does exists, get the filtered results and pass it to DataContext
        /// If parameter does not exists, do nothing
        /// </summary>
        /// <param name="e"></param>
        /// <summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            listBox.Opacity = 0;

            //If this is initial call to the event
            if (isFirstNavigation)
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorCountries);

                //Get index of previously selected country
                int selectedCountryIndex = Int32.Parse(IsolatedStorageSettings.
                    ApplicationSettings["selectedCountryIndex"].ToString());
                
                //Populate CountryModel with every CountryData that satisfies parameters 
                model = new CountryModel()
                {
                    Countries = await App.MobileService.GetTable<CountryData>().
                        Where(x => x.Id != selectedCountryIndex).
                        ToListAsync()
                };
              
                Random rand = new Random();

                //Convert Flag to FlagImage
                //After conversion, empty Flag property
                foreach (CountryData data in model.Countries)
                {
                    if(data.Rating == 0.0)
                        data.Rating = rand.Next(0, 5);

                    data.FlagImage = ImageConversionHelper.ToImage(data.Flag);
                    data.Flag = String.Empty;
                }

                //initialize double array for country coordinates
                countryCoordinates = new double[2];

                ProgressIndicatorHelper.SetProgressBar(false, null);
                isFirstNavigation = false;
            }

            textBoxSearch.Text = String.Empty;
            textBoxSearch.Visibility = System.Windows.Visibility.Collapsed;

            //If user has entered a search term
            if (NavigationContext.QueryString.ContainsKey("search"))
            {
                sortCounter = 0;

                string searchTerm = NavigationContext.QueryString["search"];

                //Populate CountryModel with data that satisfies search term
                //Always search entire model, not DataContext
                CountryModel filteredModel = new CountryModel() 
                {
                    Countries = model.Countries.Where(x => x.Name.Contains(searchTerm)).ToList()
                };
                //filteredModel.Countries = model.Countries.Where(x => x.Name.Contains(searchTerm)).ToList();

                DataContext = filteredModel;

                //Preserve map visibility across navigation/refresh
                if (isMapVisible)
                    map.Visibility = System.Windows.Visibility.Visible;
            }
            else
                DataContext = model;

            //Animate listBox with countries
            AnimationHelper.Fade(listBox, 1, 700, new PropertyPath(OpacityProperty));
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
        private void SetMapCenter()
        {
            map.Center = new GeoCoordinate(countryCoordinates[0], countryCoordinates[1]);
            map.ZoomLevel = ZOOM_LEVEL;
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
        void profileMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        /// <summary>
        /// Gets the countryCode for determining country latitude and longitude
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;

            if (currentlyOpenedExpander != ev.Tag.ToString())
            {
                //if (ev.IsExpanded)
                //{
                hasCoordinates = false;
                int id = Int32.Parse(ev.Tag.ToString());

                GeocodeQuery query = new GeocodeQuery()
                {
                    GeoCoordinate = new System.Device.Location.GeoCoordinate(0, 0),
                    SearchTerm = (DataContext as CountryModel).Countries.Single(x => x.Id == id).Name
                };

                query.QueryCompleted += query_QueryCompleted;
                query.QueryAsync();

                currentlyOpenedExpander = ev.Tag.ToString();
                //}
            }

            //Execute only if expander is opened
            //if (ev.IsExpanded)
            //{
            //    hasCoordinates = false;
            //    int id = Int32.Parse(ev.Tag.ToString());

            //    GeocodeQuery query = new GeocodeQuery()
            //    {
            //        GeoCoordinate = new System.Device.Location.GeoCoordinate(0, 0),
            //        SearchTerm = (DataContext as CountryModel).Countries.Single(x => x.Id == id).Name
            //    };

            //    query.QueryCompleted += query_QueryCompleted;
            //    query.QueryAsync();
            //}
        }

        /// <summary>
        /// Gets the latitude and longitude and centers the map accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void query_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            //defensive programming, trust no one
            if (e.Result.Count > 0)
            {
                countryCoordinates[0] = e.Result[0].GeoCoordinate.Latitude;
                countryCoordinates[1] = e.Result[0].GeoCoordinate.Longitude;

                if (map.Visibility == System.Windows.Visibility.Visible)
                    SetMapCenter();

                hasCoordinates = true;
            }
        }

        /// <summary>
        /// Shortcut for selecting country
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderView_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;

            NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml?countryId={0}&mapVisible={1}",
                ev.Tag, isMapVisible), UriKind.Relative));
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
        /// Sorts the country list by highest rating, and alphabetically (both ascending and descending)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortIconButton_Click(object sender, EventArgs e)
        {
            if (sortCounter == 0)
                DataContext = new CountryModel() { Countries = (DataContext as CountryModel).
                    Countries.OrderByDescending(x => x.Rating).ToList() };
            else if (sortCounter == 1)
                DataContext = new CountryModel() { Countries = (DataContext as CountryModel).
                    Countries.OrderByDescending(x => x.Name).ToList() };
            else
                DataContext = new CountryModel() { Countries = (DataContext as CountryModel).
                    Countries.OrderBy(x => x.Name).ToList() };

            sortCounter += 1;

            if (sortCounter == 3)
                sortCounter = 0;
        }

        /// <summary>
        /// Shows or hides map and changes the text of IconButton appropriately
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
                SetMapCenter();
            //if(countryCode != null)
            //    CoordinatesHelper.SetMapCenter(ref map, await CoordinatesHelper.GetCoordinates(countryCode, 1), ZOOM_LEVEL);
        }

        /// <summary>
        /// Selects expanded country and navigates to next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;
            
            NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml?countryId={0}&mapVisible={1}&lat={2}&lon={3}",
                bttn.Tag, isMapVisible, countryCoordinates[0], countryCoordinates[1]), UriKind.Relative));
        }
        #endregion

    }
}