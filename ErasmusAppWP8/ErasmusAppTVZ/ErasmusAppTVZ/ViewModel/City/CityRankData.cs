using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.City
{
    public class CityRankData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cityID")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "public_transport")]
        public float PublicTransport { get; set; }

        [JsonProperty(PropertyName = "air_quality")]
        public float AirQuality { get; set; }

        [JsonProperty(PropertyName = "security")]
        public float Security { get; set; }

        [JsonProperty(PropertyName = "nightlife")]
        public float Nightlife { get; set; }

        [JsonProperty(PropertyName = "life_expenses")]
        public float LifeExpenses { get; set; }

        [JsonProperty(PropertyName = "total_satisfaction")]
        public float TotalPoints { get; set; }
    }
}
