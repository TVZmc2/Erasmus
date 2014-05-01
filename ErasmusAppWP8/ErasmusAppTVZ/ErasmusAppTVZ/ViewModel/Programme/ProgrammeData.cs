using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.Programme
{
    class ProgrammeData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "facultyID")]
        public int UniversityId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}

