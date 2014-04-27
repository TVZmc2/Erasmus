using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.City
{
    public class CityData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName="countryId")]
        public int CountryId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public float Rating { get; set; }
        
    }
}
