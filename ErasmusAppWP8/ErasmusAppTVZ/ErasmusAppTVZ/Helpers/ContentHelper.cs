using ErasmusAppTVZ.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ErasmusAppTVZ.Helpers
{
    class ContentHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentName"></param>
        /// <returns></returns>
        public static CustomMessageBox GetStudentsPopUp(string studentName, string content)
        {
            string[] studentInfo = content.Split(';');

            string[] basicInfo = studentInfo[0].Split(',');
            string[] languageInfo = studentInfo[1].Split(',');
            string[] interestsInfo = studentInfo[2].Split(',');
            string[] contactInfo = studentInfo[3].Split(',');


            Grid grid = new Grid() { Margin = new Thickness(12, 12, 0, 0) };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());


            //first main row
            TextBlock header = new TextBlock() 
            {
                Text = studentName,
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeLarge"].ToString())
            };

            //add first main row content
            grid.Children.Add(header);


            //second main row
            Grid secondRowGrid = new Grid() { Margin = new Thickness(-48, 24, 0, 0) };
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
                Text = string.Format("Date of birth: {0}\nHometown: {1}\nCountry: {2}\nUniversity: {3}",
                    basicInfo[0], basicInfo[1], basicInfo[2], basicInfo[3]),
                Margin = new Thickness(-12, 0, 0, 0)
            };

            Grid.SetColumn(info, 1);

            //add columns content
            secondRowGrid.Children.Add(image);
            secondRowGrid.Children.Add(info);

            Grid.SetRow(secondRowGrid, 1);

            //add second main row content
            grid.Children.Add(secondRowGrid);


            //third main row
            Grid thirdRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            thirdRowGrid.RowDefinitions.Add(new RowDefinition());
            thirdRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock languages = new TextBlock()
            { 
                Text = "Languages",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
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

            Grid.SetRow(thirdRowGrid, 2);

            //add third main row content
            grid.Children.Add(thirdRowGrid);


            //fourth main row
            Grid fourthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock interests = new TextBlock()
            {
                Text = "Interests",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
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

            Grid.SetRow(fourthRowGrid, 3);

            //add fourth main row content
            grid.Children.Add(fourthRowGrid);


            //fifth main row
            Grid fifthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fifthRowGrid.RowDefinitions.Add(new RowDefinition());
            fifthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock contacts = new TextBlock()
            {
                Text = "Contacts",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
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

            Grid.SetRow(fifthRowGrid, 4);

            //add fifth main row content
            grid.Children.Add(fifthRowGrid);


            CustomMessageBox customMessageBox = new CustomMessageBox() 
            {
                Content = grid,
                LeftButtonContent = "ok",
                IsFullScreen = true
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