using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.University
{
    class UniversityProgrammeData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName="facultyID")]
        public int UniversityId { get; set; }

        [JsonProperty(PropertyName="programme_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName="programme_code")]
        public string Code { get; set; }
    }
}
