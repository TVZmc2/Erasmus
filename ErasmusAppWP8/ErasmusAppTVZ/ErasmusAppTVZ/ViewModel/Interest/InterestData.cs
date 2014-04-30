using Newtonsoft.Json;

namespace ErasmusAppTVZ.ViewModel.Interest
{
    class InterestData
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName="interest")]
        public string InterestName { get; set; }
    }
}
