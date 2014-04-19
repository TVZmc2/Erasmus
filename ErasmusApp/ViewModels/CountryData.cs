using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ErasmusApp.ViewModels
{
    public class CountryData
    {
        public string Name { get; set; }
        public float Grade { get; set; }
        public SolidColorBrush Flag { get; set; }
        public float CostOfLiving { get; set; }
        public float CrimeIndex { get; set; }
        public float HealthCareIndex { get; set; }
        public float PollutionIndex { get; set; }
    }
}
