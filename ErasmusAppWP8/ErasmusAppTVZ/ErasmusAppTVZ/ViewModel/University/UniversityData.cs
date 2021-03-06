﻿using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.University
{
    public class UniversityData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "cityID")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "countryID")]
        public int CountryId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public decimal Rating { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        public UniversityRankData Rank { get; set; }
    }
}
