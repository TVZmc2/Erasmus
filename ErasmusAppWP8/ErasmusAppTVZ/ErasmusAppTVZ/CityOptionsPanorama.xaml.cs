using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ErasmusAppTVZ.ViewModel.Panorama;

namespace ErasmusAppTVZ
{
    public partial class CityOptionsPanorama : PhoneApplicationPage
    {
        private PanoramaModel panoramaData;

        public CityOptionsPanorama()
        {
            InitializeComponent();

            panoramaData = new PanoramaModel();

            DataContext = panoramaData;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string cityName = NavigationContext.QueryString["cityName"];
            MainPanorama.Title = cityName;
        }
    }
}