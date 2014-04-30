using ErasmusAppTVZ.Helpers;
using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.City;
using ErasmusAppTVZ.ViewModel.Event;
using ErasmusAppTVZ.ViewModel.Interest;
using ErasmusAppTVZ.ViewModel.Language;
using ErasmusAppTVZ.ViewModel.Panorama;
using ErasmusAppTVZ.ViewModel.Student;
using ErasmusAppTVZ.ViewModel.University;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace ErasmusAppTVZ
{
    public partial class CityOptionsPanorama : PhoneApplicationPage
    {
        private PanoramaModel panoramaData;
        private int selectedCityIndex = 0;

        public CityOptionsPanorama()
        {
            InitializeComponent();

            //panoramaData = new PanoramaModel();

            //DataContext = panoramaData;

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            selectedCityIndex = Int32.Parse(NavigationContext.QueryString["cityId"]);

            panoramaData = new PanoramaModel() 
            {
                Universities = await App.MobileService.GetTable<UniversityData>().
                    Where(x => x.CityId == selectedCityIndex).ToListAsync(),
                Students = await App.MobileService.GetTable<StudentData>().
                    Where(x => x.DestinationCityId == selectedCityIndex).ToListAsync(),
                Events = await App.MobileService.GetTable<EventData>().
                    Where(x => x.CityId == selectedCityIndex).ToListAsync()
            };

            DataContext = panoramaData;

            List<string> cityName = await App.MobileService.GetTable<CityData>().
                Where(x => x.ID == selectedCityIndex).Select(x => x.Name).ToListAsync();

            MainPanorama.Title = cityName.ElementAt(0);

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

            //CustomMessageBox customMessageBox = ContentHelper.GetStudentProfileEditor(true, Interests, Languages);

            //customMessageBox.Show();
            //if (IsolatedStorageSettings.ApplicationSettings.Contains("hasProfile"))
            //    profileEditor.LeftButtonContent = AppResources.ProfileEditTitle;
            //else
            //    profileEditor.LeftButtonContent = AppResources.ProfileCreatorTitle;

            //profileEditor.RightButtonContent = "cancel";
            MainPanorama.Visibility = System.Windows.Visibility.Collapsed;
            profileViewer.Visibility = System.Windows.Visibility.Collapsed;
            profileEditor.Visibility = System.Windows.Visibility.Visible;
            //profileEditor.Show();
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
        /// <param name="data"></param>
        /// <returns></returns>
        private string FormatDataForViewing(StudentData data)
        {
            string formattedData = string.Format("{0},{1},{2},{3};",
                data.Age, data.HomeCity, "Country 1", "Faculty 1");

            if (data.Languages != null && data.Languages.Length > 0)
            {
                formattedData += data.Languages.Replace(';', ',');
                formattedData = formattedData.Substring(0, formattedData.Length - 1);
                formattedData += ";";
            }
            else
                formattedData += "Languages are not defined;";

            if (data.Interests != null && data.Interests.Length > 0)
            {
                formattedData += data.Interests.Replace(';', ',');
                formattedData = formattedData.Substring(0, formattedData.Length - 1);
                formattedData += ";";
            }
            else
                formattedData += "Interests are not defined;";

            if (data.Facebook != null && data.Facebook.Length > 0)
                formattedData += data.Facebook + ",";
            else
                formattedData += String.Empty + ",";

            if (data.Twitter != null && data.Twitter.Length > 0)
                formattedData += data.Twitter + ";";
            else
                formattedData += String.Empty + ";";

            //string content = "22,London,Great Britain,Cambridge;" +
            //                 "English,German,Spanish,Danish,Croatian,Italian,Slovenian;" +
            //                 "Science fiction,Astronomy,Sports,Physics,Mathematics,Cars,Books,Cooking;" +
            //                 "someGuy,blabla;";
                

            return formattedData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridStudents_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int id = Int32.Parse((sender as Grid).Tag.ToString());

            StudentData data = panoramaData.Students.First(x => x.ID == id);

            string content =  FormatDataForViewing(data);

            //CustomMessageBox customMessageBox = ContentHelper.GetStudentProfileViewer(
            //    string.Format("{0} {1}", data.FirstName, data.LastName), content);

            //customMessageBox.Show();
            //profileViewer.LeftButtonContent = "ok";
            //profileViewer.Show();
            MainPanorama.Visibility = System.Windows.Visibility.Collapsed;
            profileViewer.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonProfileViewer_Click(object sender, RoutedEventArgs e)
        {
            profileViewer.Visibility = System.Windows.Visibility.Collapsed;
            MainPanorama.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancelProfile_Click(object sender, RoutedEventArgs e)
        {
            profileEditor.Visibility = System.Windows.Visibility.Collapsed;
            MainPanorama.Visibility = System.Windows.Visibility.Visible;
        }

    }//class
}//namespace