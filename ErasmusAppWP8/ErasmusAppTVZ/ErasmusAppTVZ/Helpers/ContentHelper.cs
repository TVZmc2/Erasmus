using ErasmusAppTVZ.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        public static CustomMessageBox GetStudentsPopUp(string studentName)
        {
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
                Text = string.Format("Date of birth: NN\nHometown: NN\nCountry: NN\nUniversity: NN"),
                Margin = new Thickness(-12, 0, 0, 0)
            };

            Grid.SetColumn(info, 2);

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

            //add rows content
            thirdRowGrid.Children.Add(languages);

            Grid.SetRow(thirdRowGrid, 2);

            //add third main row content
            grid.Children.Add(thirdRowGrid);


            //fourth main row
            Grid fourthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock interest = new TextBlock()
            {
                Text = "Interests",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
            };

            //add rows content
            fourthRowGrid.Children.Add(interest);

            Grid.SetRow(fourthRowGrid, 3);

            //add fourth main row content
            grid.Children.Add(fourthRowGrid);


            //fifth main row
            Grid fifthRowGrid = new Grid() { Margin = new Thickness(0, 24, 0, 0) };
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());
            fourthRowGrid.RowDefinitions.Add(new RowDefinition());

            //first row
            TextBlock contacts = new TextBlock()
            {
                Text = "Contacts",
                FontSize = double.Parse(Application.Current.Resources["PhoneFontSizeMediumLarge"].ToString())
            };

            //add rows content
            fifthRowGrid.Children.Add(contacts);

            Grid.SetRow(fifthRowGrid, 5);

            //add fifth main row content
            grid.Children.Add(fifthRowGrid);



            CustomMessageBox customMessageBox = new CustomMessageBox() 
            {
                Content = grid
                //LeftButtonContent = "ok"
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