using Newtonsoft.Json;
<<<<<<< HEAD
=======

>>>>>>> c431c8a64e227f1fade538d206e972b23a19803a
namespace ErasmusAppTVZ.ViewModel.University
{
    public class UniversityData
    {
<<<<<<< HEAD
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "cityID")]
        public int cityID { get; set; }

        [JsonProperty(PropertyName = "countryID")]
        public int CountryID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public decimal Rating { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        public UniversityRankData Rank { get; set; }
=======
        public int ID { get; set; }

        [JsonProperty(PropertyName = "cityID")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "countryID")]
        public int CountryId { get; set; }

        [JsonProperty(PropertyName = "category")]
        public char Category { get; set; }

        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public float Rating { get; set; }
>>>>>>> c431c8a64e227f1fade538d206e972b23a19803a
    }
}
