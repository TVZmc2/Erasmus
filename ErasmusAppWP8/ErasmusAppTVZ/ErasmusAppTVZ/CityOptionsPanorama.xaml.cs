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
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Profile.xaml?isViewing={0}", false), UriKind.Relative));
        }

        /// <summary>
        /// Creates a calendar entry for the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonReminder_Click(object sender, RoutedEventArgs e)
        {
            Button bttn = sender as Button;

            List<EventData> Event = await App.MobileService.GetTable<EventData>().Where(x => x.ID == Convert.ToInt32(bttn.Tag)).ToListAsync();
            EventData ed = new EventData();

            ed = Event.ElementAt(0);
           
            SaveAppointmentTask saveAppointment = new SaveAppointmentTask();

            saveAppointment.StartTime = ed.Date;
            saveAppointment.EndTime = ed.Date.AddMinutes(Convert.ToDouble(ed.Duration));
            saveAppointment.Subject = ed.Title;
            saveAppointment.Location = ed.Location;
            saveAppointment.Details = ed.Description;
            saveAppointment.Reminder = Reminder.OneHour;
            saveAppointment.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;

            saveAppointment.Show();
        }

        /// <summary>
        /// Formats student data for profile viewer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string FormatDataForViewing(StudentData data)
        {
            string formattedData = string.Format("{0},{1},{2},{3},{4},{5};",
                data.FirstName, data.LastName, data.Age, data.HomeCity, "Country 1", "Faculty 1");

            if (data.Languages != null && data.Languages.Length > 0)
                formattedData += data.Languages + ";";
            else
                formattedData += String.Empty + ";";

            if (data.Interests != null && data.Interests.Length > 0)
                formattedData += data.Interests + ";";
            else
                formattedData += String.Empty + ";";

            if (data.Facebook != null && data.Facebook.Length > 0)
                formattedData += data.Facebook + ";";
            else
                formattedData += String.Empty + ";";

            if (data.Twitter != null && data.Twitter.Length > 0)
                formattedData += data.Twitter + ";";
            else
                formattedData += String.Empty + ";";
                
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

            string content = FormatDataForViewing(data);

            NavigationService.Navigate(new Uri(string.Format("/Profile.xaml?isViewing={0}&content={1}",
                true, content), UriKind.Relative));
        }

    }
}