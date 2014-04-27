using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErasmusAppTVZ.ViewModel.Programme
{
    class ProgrammeData
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "facultyID")]
        public int facultyID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}
