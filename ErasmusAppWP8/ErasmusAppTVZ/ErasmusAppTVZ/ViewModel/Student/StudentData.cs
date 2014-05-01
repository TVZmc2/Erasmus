using System.Collections.Generic;
using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.Student
{
    //public enum SocialNetworks
    //{
    //    Facebook = 1,
    //    Twitter = 2,
    //    GooglePlus = 3
    //}

    public class StudentData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "home_city")]
        public string HomeCity { get; set; }

        [JsonProperty(PropertyName = "home_faculty")]
        public string HomeUniversity { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "destination_faculty")]
        public string University { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "facebook")]
        public string Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter")]
        public string Twitter { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Languages { get; set; }

        [JsonProperty(PropertyName = "interests")]
        public string Interests { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "destination_cityID")]
        public int DestinationCityId { get; set; }
    }
}
