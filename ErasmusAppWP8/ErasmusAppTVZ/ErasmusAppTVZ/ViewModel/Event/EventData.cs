using Newtonsoft.Json;
using System;

namespace ErasmusAppTVZ.ViewModel.Event
{
    public class EventData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName="cityID")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "date_time")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }
    }
}
