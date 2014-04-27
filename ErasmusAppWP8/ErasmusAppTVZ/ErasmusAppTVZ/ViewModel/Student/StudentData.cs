using System.Collections.Generic;
using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.Student
{
    public enum SocialNetworks
    {
        Facebook = 1,
        Twitter = 2,
        GooglePlus = 3
    }

    public class StudentData
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "destination_faculty")]
        public string University { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public char Sex { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        public Dictionary<SocialNetworks, string> SocialContacts;

        [JsonProperty(PropertyName = "languages")]
        public string Languages { get; set; }

        [JsonProperty(PropertyName = "interests")]
        public string Interests { get; set; }
    }
}
