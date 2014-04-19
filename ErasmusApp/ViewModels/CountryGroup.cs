using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ErasmusApp.ViewModels
{
    public class CountryGroup
    {
        public string Title { get; set; }
        public List<CountryData> Information { get; set; }
    }
}
