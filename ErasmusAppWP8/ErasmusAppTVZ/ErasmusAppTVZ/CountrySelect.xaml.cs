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
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using ErasmusAppTVZ.Helpers;

namespace ErasmusAppTVZ
{
    public partial class CountrySelect : PhoneApplicationPage
    {
        Grid expandedItem;
        ApplicationBarIconButton showOnMapIconButton;
        private static bool isFirstNavigation = true;
        private static CountryModel model;


        public CountrySelect()
        {
            InitializeComponent();

            //TODO: remove App.ViewModel and create CountryModel object
            //DataContext = App.ViewModel;

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Shows or hides Progress indicator
        /// </summary>
        /// <param name="check"></param>
        private void SetProgressBar(bool check)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = check;
            SystemTray.ProgressIndicator.IsVisible = check;
        }

        /// <summary>
        /// Checks if 'search' parameter exists
        /// If parameter does exists, get the filtered results and pass it to DataContext
        /// If parameter does not exists, do nothing
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            listBox.Opacity = 0;

            if (isFirstNavigation)
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SetProgressBar(true);

                model = new CountryModel() 
                {
                    Countries = await App.MobileService.GetTable<CountryData>().ToListAsync()
                };

                foreach (CountryData data in model.Countries)
                {
                    byte[] buffer = Convert.FromBase64String(data.Flag);

                    using (MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length))
                    {
                        ms.Write(buffer, 0, buffer.Length);
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(ms);

                        data.FlagImage = bitmap;
                    }
                }

                SetProgressBar(false);
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

            AnimationHelper.Animate(listBox, 1, 2000, new PropertyPath(OpacityProperty));
        }

        /// <summary>
        /// Build a localized application bar with icons and menu items
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton searchIconButton = new ApplicationBarIconButton();
            showOnMapIconButton = new ApplicationBarIconButton();
            searchIconButton.Text = AppResources.ApplicationBarSearch;
            showOnMapIconButton.Text = AppResources.ApplicationBarHideMap;
            searchIconButton.IconUri = new Uri("/Assets/AppBar/search.png", UriKind.Relative);
            showOnMapIconButton.IconUri = new Uri("/Assets/AppBar/map.png", UriKind.Relative);
            searchIconButton.Click += searchIconButton_Click;
            showOnMapIconButton.Click += showOnMapIconButton_Click;

            ApplicationBar.Buttons.Add(searchIconButton);
            ApplicationBar.Buttons.Add(showOnMapIconButton);
        }

        /// <summary>
        /// Show or hide map and change the text of IconButton appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showOnMapIconButton_Click(object sender, EventArgs e)
        {
            if (map.Visibility == System.Windows.Visibility.Visible)
            {
                map.Visibility = System.Windows.Visibility.Collapsed;
                showOnMapIconButton.Text = AppResources.ApplicationBarShowMap;
                return;
            }

            showOnMapIconButton.Text = AppResources.ApplicationBarHideMap;
            map.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Check if textBoxSearch.Text.Length is greater than 0
        /// If yes, than refresh page with search parameter
        /// If not, show TextBox and get focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void searchIconButton_Click(object sender, EventArgs e)
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
        private void ExpandedContentButton_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;

            NavigationService.Navigate(new Uri(string.Format("/CitySelect.xaml?countryId={0}", bttn.Tag),
                UriKind.Relative));
        }

        /// <summary>
        /// Expands or collapses items depending on their current visibillity
        /// </summary>
        /// <param name="expandedItem">Holds reference to last expanded item</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grid = (sender as Grid).Children[3] as Grid;

            if (expandedItem != null)
            {
                expandedItem.Visibility = System.Windows.Visibility.Collapsed;

                if (expandedItem == grid)
                {
                    expandedItem = null;
                    return;
                }
            }

            if (grid.Visibility == System.Windows.Visibility.Collapsed)
            {
                expandedItem = grid;
                grid.Visibility = System.Windows.Visibility.Visible;
                return;
            }
        }

    }
}