using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.City;
using ErasmusAppTVZ.ViewModel.Country;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class CountrySelect : PhoneApplicationPage
    {
        private static bool isFirstNavigation = true;
        private static CountryModel model;
        private static int sortCounter = 0;

        public CountrySelect()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Sets the visibillity and indertermination of ProgressIndicator
        /// </summary>
        /// <param name="check"></param>
        //private void SetProgressBar(bool check)
        //{
        //    SystemTray.ProgressIndicator.Text = AppResources.ProgressIndicatorCountries;
        //    SystemTray.ProgressIndicator.IsIndeterminate = check;
        //    SystemTray.ProgressIndicator.IsVisible = check;
        //}

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

            if (isFirstNavigation)
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                ProgressIndicatorHelper.SetProgressBar(true, AppResources.ProgressIndicatorCountries);

                model = new CountryModel()
                {
                    Countries = await App.MobileService.GetTable<CountryData>().ToListAsync()
                };

                Random rand = new Random();
                foreach (CountryData data in model.Countries)
                {
                    data.Rating = rand.Next(0, 5);
                    data.FlagImage = ImageConversionHelper.ToImage(data.Flag);
                }
                
                ProgressIndicatorHelper.SetProgressBar(false, null);
                isFirstNavigation = false;
            }

            textBoxSearch.Text = String.Empty;
            textBoxSearch.Visibility = System.Windows.Visibility.Collapsed;

            if (NavigationContext.QueryString.ContainsKey("search"))
            {
                string searchTerm = NavigationContext.QueryString["search"];

                CountryModel filteredModel = new CountryModel();
                filteredModel.Countries = model.Countries.Where(x => x.Name.Contains(searchTerm)).ToList();

                DataContext = filteredModel;
            }
            else
                DataContext = model;

            AnimationHelper.Fade(listBox, 1, 750, new PropertyPath(OpacityProperty));
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
        void profileMenuItem_Click(object sender, EventArgs e)
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
        private void ExpanderView_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ExpanderView ev = sender as ExpanderView;

            NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml?countryId={0}", ev.Tag),
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
            CountryModel cm;

            if (sortCounter == 0)
                 cm = new CountryModel() { Countries = model.Countries.OrderByDescending(x => x.Rating).ToList() };
            else if (sortCounter == 1)
                cm = new CountryModel() { Countries = model.Countries.OrderByDescending(x => x.Name).ToList() };
            else
                cm = new CountryModel() { Countries = model.Countries.OrderBy(x => x.Name).ToList() };

            sortCounter += 1;

            if (sortCounter == 3)
                sortCounter = 0;

            DataContext = cm;
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
                NavigationService.Navigate(new Uri(string.Format("/CountrySelect.xaml" +
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

            NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml?countryId={0}", bttn.Tag),
                UriKind.Relative));
        }
        #endregion

    }//class
}//namespace