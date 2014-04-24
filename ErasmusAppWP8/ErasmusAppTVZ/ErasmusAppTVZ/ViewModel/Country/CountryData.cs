using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.Country
{
    public class CountryData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public float Rating { get; set; }

        [JsonProperty(PropertyName = "flag")]
        public string Flag { get; set; }
    }
}
