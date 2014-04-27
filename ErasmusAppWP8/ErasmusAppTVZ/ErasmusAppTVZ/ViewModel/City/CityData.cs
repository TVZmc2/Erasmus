using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.City
{
    public class CityData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName="countryId")]
        public int CountryId { get; set; }

        [JsonProperty(PropertyName = "city_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "city_rating")]
        public float Rating { get; set; }
        
    }
}
