using ErasmusAppTVZ.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ErasmusAppTVZ.Helpers
{
    class ApplicationBarHelper
    {
        /// <summary>
        /// Formats the content of the Grid for CustomMessageBox
        /// </summary>
        /// <returns>Grid</returns>
        public static Grid GetAboutContentGrid()
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

            return grid;
        }
    }
}
