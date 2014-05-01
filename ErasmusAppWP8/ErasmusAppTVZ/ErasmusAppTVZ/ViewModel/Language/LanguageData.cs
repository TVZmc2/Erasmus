using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ErasmusAppTVZ.ViewModel.Language
{
    class LanguageData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName="title")]
        public string Name { get; set; }

        [JsonProperty(PropertyName="flag")]
        public string Flag { get; set; }

        //public BitmapImage FlagImage { get; set; }
    }
}
