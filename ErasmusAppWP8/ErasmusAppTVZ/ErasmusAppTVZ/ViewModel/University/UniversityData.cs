using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.University
{
    class UniversityData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cityId")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName="faculty_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "faculty_rating")]
        public float Rating { get; set; }
    }
}
