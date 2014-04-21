using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ErasmusApp.Models.City;

namespace ErasmusApp
{
    public partial class CitySelect : PhoneApplicationPage
    {
        public CitySelect()
        {
            InitializeComponent();

            CityModel cm = new CityModel();
            cm.LoadData();

            DataContext = cm;
        }
    }
}