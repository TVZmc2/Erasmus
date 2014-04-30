using ErasmusAppTVZ.Resources;
using ErasmusAppTVZ.ViewModel.Interest;
using ErasmusAppTVZ.ViewModel.Language;
using ErasmusAppTVZ.ViewModel.Student;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ErasmusAppTVZ.Helpers
{
    sealed class ContentHelper
    {
        private static List<string> studentInterests;
        private static List<string> studentLanguages;
        private static List<InterestData> Interests;
        private static List<LanguageData> Languages;
        private static string gender;
        private static SolidColorBrush backgroundColor;

        private static TextBlock GetTextBlock(string text)
        {
            TextBlock tb = new TextBlock() 
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
            };

            return tb;
        }

        /// <summary>
        /// Generates content for profile editor
        /// </summary>
        /// <returns></returns>
        public static CustomMessageBox GetStudentProfileEditor(bool hasProfile, List<InterestData> interests, List<LanguageData> languages) 
        {
            Interests = interests;
            Languages = languages;

            CustomMessageBox customMessageBox = new CustomMessageBox();

            backgroundColor = customMessageBox.Background as SolidColorBrush;

            ScrollViewer scrollViewer = new ScrollViewer() 
            { 
                Height = App.Current.Host.Content.ActualHeight - 100,
                Margin = new Thickness(12, 0, 0, 0)
            };
            StackPanel stackPanel = new StackPanel();

            TextBlock headerTextBlock = new TextBlock() 
            {
                Text = hasProfile ? AppResources.ProfileEditTitle : AppResources.ProfileCreatorTitle,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeLarge"].ToString())
            };

            stackPanel.Children.Add(headerTextBlock);


            TextBlock textBlockName = GetTextBlock(AppResources.ProfileName);

            TextBox textBoxName = new TextBox() { Margin = new Thickness(-12, 0, 0, 0) };

            stackPanel.Children.Add(textBlockName);
            stackPanel.Children.Add(textBoxName);


            TextBlock textBlockGender = GetTextBlock(AppResources.ProfileGender);
            stackPanel.Children.Add(textBlockGender);

            Grid genderGrid = new Grid();
            genderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            genderGrid.ColumnDefinitions.Add(new ColumnDefinition());

            RadioButton radioButtonM = new RadioButton() { Content = "M" };
            radioButtonM.Click += (senderM, eM) =>
                { gender = radioButtonM.Content.ToString(); };
            RadioButton radioButtonF = new RadioButton() { Content = "F" };
            radioButtonF.Click += (senderM, eM) =>
                { gender = radioButtonF.Content.ToString(); };

            Grid.SetColumn(radioButtonF, 1);

            genderGrid.Children.Add(radioButtonM);
            genderGrid.Children.Add(radioButtonF);

            stackPanel.Children.Add(genderGrid);

            TextBlock textBlockDob = GetTextBlock(AppResources.ProfileAge);

            TextBox textBoxAge = new TextBox() { Margin = new Thickness(-12, 0, 0, 0) };

            stackPanel.Children.Add(textBlockDob);
            stackPanel.Children.Add(textBoxAge);


            TextBlock textBlockHometown = GetTextBlock(AppResources.ProfileHomeCity);
            TextBox textBoxHomeTown = new TextBox() { Margin = new Thickness(-12, 0, 0, 0) };
            stackPanel.Children.Add(textBlockHometown);
            stackPanel.Children.Add(textBoxHomeTown);


            //TextBlock textBlockHomeUniversity = GetTextBlock(AppResources.ProfileHomeUniversity);
            //TextBox textBoxHomeUniversity = new TextBox() { Margin = new Thickness(-12, 0, 0, 0) };
            //stackPanel.Children.Add(textBlockHomeUniversity);
            //stackPanel.Children.Add(textBoxHomeUniversity);


            TextBlock textBlockLanguages = GetTextBlock(AppResources.ProfileLanguages);

            Grid languagesGrid = new Grid() { Margin = new Thickness(-12, 0, 0, 0) };
            languagesGrid.RowDefinitions.Add(new RowDefinition());
            Button bttn;

            int width = 160;
            int column = 0;
            int row = 0;
            for (int i = 0; i < Languages.Count; i++)
            {
                width += 160;
                languagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
                languagesGrid.ColumnDefinitions[i].Width = new GridLength(160);

                bttn = new Button()
                {
                    Content = Languages[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderThickness = new Thickness(1),
                    Tag = Languages[i].ID
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

            stackPanel.Children.Add(textBlockLanguages);
            stackPanel.Children.Add(languagesGrid);

            TextBlock textBlockInterests = GetTextBlock(AppResources.ProfileInterests);

            Grid interestsGrid = new Grid() { Margin = new Thickness(-12, 0, 0, 0) };
            interestsGrid.RowDefinitions.Add(new RowDefinition());

            width = 160;
            column = 0;
            row = 0;
            for (int i = 0; i < Interests.Count; i++)
            {
                width += 160;
                interestsGrid.ColumnDefinitions.Add(new ColumnDefinition());
                interestsGrid.ColumnDefinitions[i].Width = new GridLength(160);

                bttn = new Button() 
                {
                    Content = Interests[i].InterestName,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderThickness = new Thickness(1),
                    Tag = Interests[i].ID
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

            stackPanel.Children.Add(textBlockInterests);
            stackPanel.Children.Add(interestsGrid);


            TextBlock textBlockContacts = GetTextBlock(AppResources.ProfileContacts);
            TextBox textBoxFacebook = new TextBox() { Text = "Facebook channel name" };
            textBoxFacebook.GotFocus += (senderFacebook, eFacebook) =>
                {
                    if (textBoxFacebook.Text.Contains(" "))
                        textBoxFacebook.Text = String.Empty;
                };
            TextBox textBoxTwitter = new TextBox() { Text = "Twitter channel name" };
            textBoxTwitter.GotFocus += (senderTwitter, eTwitter) =>
            {
                if (textBoxTwitter.Text.Contains(" "))
                    textBoxTwitter.Text = String.Empty;
            };

            stackPanel.Children.Add(textBlockContacts);
            stackPanel.Children.Add(textBoxFacebook);
            stackPanel.Children.Add(textBoxTwitter);


            scrollViewer.Content = stackPanel;


            customMessageBox.Content = scrollViewer;
            customMessageBox.LeftButtonContent = AppResources.ProfileCreatorTitle.Substring(0, AppResources.ProfileCreatorTitle.IndexOf(" "));
            customMessageBox.RightButtonContent = AppResources.ApplicationCancel;

            customMessageBox.Dismissed += async (sender, e) =>
                {
                    if (e.Result == CustomMessageBoxResult.LeftButton)
                    {
                        StudentData studentData = new StudentData();

                        studentData.FirstName = textBoxName.Text.Substring(0, textBoxName.Text.IndexOf(" "));
                        studentData.LastName = textBoxName.Text.Substring(textBoxName.Text.IndexOf(" ") + 1);
                        studentData.Age = Int32.Parse(textBoxAge.Text);
                        studentData.HomeCity = textBoxHomeTown.Text;

                        //string[] preferences = JsonConvert.DeserializeObject<string[]>
                        //(IsolatedStorageSettings.ApplicationSettings["preferences"].ToString());
                        //studentData.HomeUniversity = preferences[2];
                        studentData.HomeUniversity = "Test University";

                        studentData.Gender = gender;
                        studentData.Facebook = textBoxFacebook.Text.Contains(" ") ? String.Empty : textBoxFacebook.Text;
                        studentData.Twitter = textBoxTwitter.Text.Contains(" ") ? String.Empty : textBoxTwitter.Text;

                        //test
                        studentData.Email = "test@test.com";
                        studentData.Image = String.Empty;
                        studentData.University = String.Empty;
                        studentData.DestinationCityID = 4;

                        foreach (string data in studentLanguages)
                            studentData.Languages += data + ";";

                        foreach (string data in studentInterests)
                            studentData.Interests += data + ";";

                        //studentData.ID = 2;

                        //await App.MobileService.GetTable<StudentData>().InsertAsync(studentData);
                    }
                    else
                    {
                        customMessageBox.Show();
                        System.Diagnostics.Debug.WriteLine("cancelled");
                    }
                };

            return customMessageBox;
        }

        private static void languagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentLanguages == null)
                studentLanguages = new List<string>();

            int id = Int32.Parse((sender as Button).Tag.ToString());

            if (!studentLanguages.Contains(id.ToString()))
            {
                studentLanguages.Add(id.ToString());
                Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
                (sender as Button).Background = new SolidColorBrush(currentAccentColorHex);
            }
            else
            {
                (sender as Button).Background = backgroundColor;
                studentLanguages.Remove(id.ToString());
            }

        }

        private static void interestButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentInterests == null)
                studentInterests = new List<string>();

            int id = Int32.Parse((sender as Button).Tag.ToString());

            if (!studentInterests.Contains(id.ToString()))
            {
                studentInterests.Add(id.ToString());
                Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
                (sender as Button).Background = new SolidColorBrush(currentAccentColorHex);
            }
            else
            {
                studentInterests.Remove(id.ToString());
                (sender as Button).Background = backgroundColor;
            }
        }

        /// <summary>
        /// Generates content for profile viewer
        /// </summary>
        /// <param name="studentName"></param>
        /// <returns></returns>
        public static CustomMessageBox GetStudentProfileViewer(string studentName, string content)
        {
            string[] studentInfo = content.Split(';');

            string[] basicInfo = studentInfo[0].Split(',');
            string[] languageInfo = studentInfo[1].Split(',');
            string[] interestsInfo = studentInfo[2].Split(',');
            string[] contactInfo = studentInfo[3].Split(',');

            ScrollViewer scrollViewer = new ScrollViewer() { Height = App.Current.Host.Content.ActualHeight - 100 };
            StackPanel stackPanel = new StackPanel();

            //header
            TextBlock header = new TextBlock() 
            {
                Text = studentName,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeLarge"].ToString())
            };

            //add header
            stackPanel.Children.Add(header);


            //image and basic info
            Grid secondRowGrid = new Grid() { Margin = new Thickness(-54, 24, 0, 0) };
            secondRowGrid.ColumnDefinitions.Add(new ColumnDefinition());
            secondRowGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //first column
            Rectangle image = new Rectangle() 
            {
                Fill = new SolidColorBrush(Colors.Gray),
                Width = 150
            };

            //second column
            TextBlock info = new TextBlock() 
            {
                Text = string.Format("Age: {0}\nHometown: {1}\nCountry: {2}\nUniversity: {3}",
                    basicInfo[0], basicInfo[1], basicInfo[2], basicInfo[3]),
                Margin = new Thickness(-12, 0, 0, 0)
            };

            Grid.SetColumn(info, 1);

            //add columns content
            secondRowGrid.Children.Add(image);
            secondRowGrid.Children.Add(info);

            //add image and basic info
            stackPanel.Children.Add(secondRowGrid);


            //languages
            Grid thirdRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            thirdRowGrid.RowDefinitions.Add(new RowDefinition());
            thirdRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock languages = new TextBlock()
            { 
                Text = "Languages",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString()),
                FontWeight = FontWeights.Bold
            };

            //second row
            int width = 100;
            int column = 0;
            int row = 0;
            Grid thirdRowHelperGrid = new Grid();
            thirdRowHelperGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < languageInfo.Length; i++)
            {
                width += 100;
                thirdRowHelperGrid.ColumnDefinitions.Add(new ColumnDefinition());
                thirdRowHelperGrid.ColumnDefinitions[i].Width = new GridLength(100);

                TextBlock language = new TextBlock() 
                {
                    Text = languageInfo[i],
                    Margin = new Thickness(0, 12, 12, 0),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Grid.SetColumn(language, column);
                Grid.SetRow(language, row);
                thirdRowHelperGrid.Children.Add(language);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    thirdRowHelperGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 0;
                }
            }

            Grid.SetRow(thirdRowHelperGrid, 1);

            //add rows content
            thirdRowGrid.Children.Add(languages);
            thirdRowGrid.Children.Add(thirdRowHelperGrid);

            //add languages
            stackPanel.Children.Add(thirdRowGrid);


            //interests
            Grid fourthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock interests = new TextBlock()
            {
                Text = "Interests",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString()),
                FontWeight = FontWeights.Bold
            };

            //second row
            width = 150;
            column = 0;
            row = 0;
            Grid fourthRowHelperGrid = new Grid();
            fourthRowHelperGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < interestsInfo.Length; i++)
            {
                width += 150;
                fourthRowHelperGrid.ColumnDefinitions.Add(new ColumnDefinition());
                fourthRowHelperGrid.ColumnDefinitions[i].Width = new GridLength(150);

                TextBlock interest = new TextBlock()
                {
                    Text = interestsInfo[i],
                    Margin = new Thickness(0, 12, 12, 0),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Grid.SetColumn(interest, column);
                Grid.SetRow(interest, row);
                fourthRowHelperGrid.Children.Add(interest);

                column += 1;

                if (width > Application.Current.Host.Content.ActualWidth)
                {
                    fourthRowHelperGrid.RowDefinitions.Add(new RowDefinition());
                    row += 1;
                    column = 0;
                    width = 150;
                }
            }

            Grid.SetRow(fourthRowHelperGrid, 1);

            //add rows content
            fourthRowGrid.Children.Add(interests);
            fourthRowGrid.Children.Add(fourthRowHelperGrid);

            //add interests
            stackPanel.Children.Add(fourthRowGrid);


            //contacts
            Grid fifthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fifthRowGrid.RowDefinitions.Add(new RowDefinition());
            fifthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock contacts = new TextBlock()
            {
                Text = "Contacts",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString()),
                FontWeight = FontWeights.Bold
            };

            //second row
            column = 0;
            Grid fifthRowHelperGrid = new Grid();
            fifthRowHelperGrid.RowDefinitions.Add(new RowDefinition());
            string baseUri = String.Empty;
            string text = String.Empty;
            for (int i = 0; i < contactInfo.Length; i++)
            {
                if (contactInfo[i].Length > 0)
                {
                    fifthRowHelperGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    fifthRowHelperGrid.ColumnDefinitions[i].Width = new GridLength(150);

                    if (i == 0)
                    {
                        text = "Facebook";
                        baseUri = "https://facebook.com/";
                    }
                    else if (i == 1)
                    {
                        text = "Twitter";
                        baseUri = "https://twitter.com/";
                    }

                    HyperlinkButton bttn = new HyperlinkButton()
                    {
                        Content = text,
                        Margin = new Thickness(-12, 12, 12, 0),
                        NavigateUri = new Uri(baseUri + contactInfo[i], UriKind.Absolute),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    Grid.SetColumn(bttn, column);
                    Grid.SetRow(bttn, row);
                    fifthRowHelperGrid.Children.Add(bttn);

                    column += 1;
                }
            }

            Grid.SetRow(fifthRowHelperGrid, 1);

            //add rows content
            fifthRowGrid.Children.Add(contacts);
            fifthRowGrid.Children.Add(fifthRowHelperGrid);

            //add contacts
            stackPanel.Children.Add(fifthRowGrid);

            //add student profile
            scrollViewer.Content = stackPanel;

            CustomMessageBox customMessageBox = new CustomMessageBox() 
            {
                Content = scrollViewer,
                LeftButtonContent = "ok",
                Height = App.Current.Host.Content.ActualHeight
            };

            return customMessageBox;
        }

        /// <summary>
        /// Creates CustomMessageBox with About content
        /// </summary>
        /// <returns>Grid</returns>
        public static CustomMessageBox GetAboutMessageBox()
        {
            XElement appInfo = XDocument.Load("WMAppManifest.xml")
                .Root.Element("App");

            string title = appInfo.Attribute("Title").Value;
            string version = appInfo.Attribute("Version").Value;
            string description = appInfo.Attribute("Description").Value;

            Grid grid = new Grid() 
            { 
                Margin = new Thickness(12, 0, 0, 0) 
            };

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            TextBlock txtBlockTitle = new TextBlock()
            {
                Text = title,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeLarge"].ToString())
            };

            TextBlock txtBlockMessage = new TextBlock()
            {
                Text = string.Format("\n{0}\nVersion: {1}",
                    description, version),
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
            };

            Grid.SetRow(txtBlockMessage, 1);
            grid.Children.Add(txtBlockTitle);
            grid.Children.Add(txtBlockMessage);

            CustomMessageBox msgBox = new CustomMessageBox() 
            {
                Caption = AppResources.ApplicationBarAboutMenuItem,
                Content = grid,
                LeftButtonContent = "ok"
            };

            return msgBox;
        }
    }
}