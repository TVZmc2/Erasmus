using ErasmusAppTVZ.ViewModel.Interest;
using ErasmusAppTVZ.ViewModel.Language;
using ErasmusAppTVZ.ViewModel.Student;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ErasmusAppTVZ
{
    public partial class Profile : PhoneApplicationPage
    {
        //Variables for storing user choice while creating or editing profile
        private List<string> studentInterests;
        private List<string> studentLanguages;
        private string gender;

        private Color themeSafeColor = Colors.White;

        public Profile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            profileEditor.Visibility = System.Windows.Visibility.Collapsed;
            profileViewer.Visibility = System.Windows.Visibility.Collapsed;

            bool isViewing = bool.Parse(NavigationContext.QueryString["isViewing"]);

            if (isViewing)
            {
                string content = NavigationContext.QueryString["content"];
                ShowData(content);
                profileViewer.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                List<LanguageData> languages;
                List<InterestData> interests;

                if (!IsolatedStorageSettings.ApplicationSettings.Contains("hasProfile"))
                {
                    languages = await App.MobileService.GetTable<LanguageData>().ToListAsync();
                    interests = await App.MobileService.GetTable<InterestData>().ToListAsync();
                }
                else
                {
                    //TODO: take languages and interests from the user data
                    languages = new List<LanguageData>();
                    interests = new List<InterestData>();
                }

                PopulateGrid(languages, interests);
                profileEditor.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Shows profile data formatted for viewing
        /// </summary>
        /// <param name="content"></param>
        private void ShowData(string content)
        {
            string[] studentInfo = content.Split(';');

            string[] basicInfo = studentInfo[0].Split(',');
            string[] languageInfo = studentInfo[1].Split(',');
            string[] interestsInfo = studentInfo[2].Split(',');
            string facebookInfo = studentInfo[3];
            string twitterInfo = studentInfo[4];

            textBoxNameViewer.Text = basicInfo[0] + " " + basicInfo[1];
            textBoxAgeViewer.Text = basicInfo[2];
            textBoxHomeTownViewer.Text = basicInfo[3];
            textBoxCountryViewer.Text = basicInfo[4];
            textBoxUniversityViewer.Text = basicInfo[5];

            int width = 100;
            int column = 0;
            int row = 0;
            languagesViewerGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < languageInfo.Length; i++)
            {
                width += 100;
                languagesViewerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                languagesViewerGrid.ColumnDefinitions[i].Width = new GridLength(100);

                TextBlock language = new TextBlock()
                {
                    Text = languageInfo[i],
                    Margin = new Thickness(0, 12, 12, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush(themeSafeColor)
                };

                Grid.SetColumn(language, column);
                Grid.SetRow(language, row);
                languagesViewerGrid.Children.Add(language);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    languagesViewerGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 0;
                }
            }

            width = 150;
            column = 0;
            row = 0;
            interestsViewerGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < interestsInfo.Length; i++)
            {
                width += 150;
                interestsViewerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                interestsViewerGrid.ColumnDefinitions[i].Width = new GridLength(150);

                TextBlock interest = new TextBlock()
                {
                    Text = interestsInfo[i],
                    Margin = new Thickness(0, 12, 12, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush(themeSafeColor)
                };

                Grid.SetColumn(interest, column);
                Grid.SetRow(interest, row);
                interestsViewerGrid.Children.Add(interest);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    interestsViewerGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 150;
                }
            }

            if (facebookInfo != String.Empty)
                facebookLink.NavigateUri = new Uri("https://facebook.com/" + facebookInfo, UriKind.Absolute);
            else
                facebookLink.Visibility = System.Windows.Visibility.Collapsed;

            if (twitterInfo != String.Empty)
            {
                twitterLink.NavigateUri = new Uri("https://twitter.com/" + twitterInfo, UriKind.Absolute);

                if (facebookInfo == String.Empty)
                    Grid.SetColumn(twitterLink, 0);
            }
            else
                twitterLink.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Populates grid with content from the lists
        /// </summary>
        /// <param name="languages">List of available languages</param>
        /// <param name="interests">List of available interests</param>
        private void PopulateGrid(List<LanguageData> languages, List<InterestData> interests)
        {
            languagesGrid.RowDefinitions.Add(new RowDefinition());

            int width = 160;
            int column = 0;
            int row = 0;
            Button bttn;
            for (int i = 0; i < languages.Count; i++)
            {
                width += 160;
                languagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
                languagesGrid.ColumnDefinitions[i].Width = new GridLength(160);

                bttn = new Button()
                {
                    Content = languages[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderThickness = new Thickness(1),
                    Tag = languages[i].Name,
                    BorderBrush = new SolidColorBrush(themeSafeColor),
                    Foreground = new SolidColorBrush(themeSafeColor)
                };

                bttn.Click += languagesButton_Click;

                Grid.SetColumn(bttn, column);
                Grid.SetRow(bttn, row);
                languagesGrid.Children.Add(bttn);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    languagesGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 160;
                }
            }

            interestsGrid.RowDefinitions.Add(new RowDefinition());

            width = 160;
            column = 0;
            row = 0;
            for (int i = 0; i < interests.Count; i++)
            {
                width += 160;
                interestsGrid.ColumnDefinitions.Add(new ColumnDefinition());
                interestsGrid.ColumnDefinitions[i].Width = new GridLength(160);

                bttn = new Button()
                {
                    Content = interests[i].InterestName,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderThickness = new Thickness(1),
                    Tag = interests[i].InterestName,
                    BorderBrush = new SolidColorBrush(themeSafeColor),
                    Foreground = new SolidColorBrush(themeSafeColor)
                };

                bttn.Click += interestButton_Click;

                Grid.SetColumn(bttn, column);
                Grid.SetRow(bttn, row);
                interestsGrid.Children.Add(bttn);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    interestsGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 160;
                }
            }
        }

        #region EventHandlers
        /// <summary>
        /// Selects or deselects language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void languagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentLanguages == null)
                studentLanguages = new List<string>();

            string name = (sender as Button).Tag.ToString();

            if (!studentLanguages.Contains(name))
            {
                studentLanguages.Add(name);
                Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
                (sender as Button).Background = new SolidColorBrush(currentAccentColorHex);
            }
            else
            {
                (sender as Button).Background = profileViewer.Background;
                studentLanguages.Remove(name);
            }

        }

        /// <summary>
        /// Selects or deselects interest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void interestButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentInterests == null)
                studentInterests = new List<string>();

            string name = (sender as Button).Tag.ToString();

            if (!studentInterests.Contains(name))
            {
                studentInterests.Add(name);
                Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
                (sender as Button).Background = new SolidColorBrush(currentAccentColorHex);
            }
            else
            {
                studentInterests.Remove(name);
                (sender as Button).Background = profileViewer.Background;
            }
        }

        /// <summary>
        /// Creates or updates user profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void ButtonCreateProfile_Click(object sender, RoutedEventArgs e)
        private void ButtonCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            StudentData data = new StudentData();

            data.FirstName = textBoxName.Text.Substring(0, textBoxName.Text.IndexOf(' '));
            data.LastName = textBoxName.Text.Substring(textBoxName.Text.IndexOf(' ') + 1);
            data.HomeCity = "City 1";
            data.HomeUniversity = "Home University 1";
            data.Gender = gender;
            data.University = "Destination University 1";
            data.Age = Int32.Parse(textBoxAge.Text);
            data.Email = textBoxEmail.Text;
            data.Facebook = phoneTextBoxFacebook.Text.Length > 0 ? phoneTextBoxFacebook.Text : String.Empty;
            data.Twitter = phoneTextBoxTwitter.Text.Length > 0 ? phoneTextBoxTwitter.Text : String.Empty;

            foreach (string item in studentLanguages)
                data.Languages += item + ",";

            data.Languages = data.Languages.Substring(0, data.Languages.Length - 1);

            foreach (string item in studentInterests)
                data.Interests += item + ",";

            data.Interests = data.Interests.Substring(0, data.Interests.Length - 1);

            data.Image = String.Empty;
            data.DestinationCityId = 26;

            //throws an error - bad request
            //await App.MobileService.GetTable<StudentData>().InsertAsync(data);
        }

        /// <summary>
        /// Cancels profile creation/edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancelProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Finishes profile viewing and goes back to CityOptionsPanorama
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonProfileViewer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Changes gender to male
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            gender = "M";
        }

        /// <summary>
        /// Changes gender to female
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            gender = "F";
        }
        #endregion
    }
}