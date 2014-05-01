using Newtonsoft.Json;

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
