using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public class ConfigSection
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("parameters")]
        public List<SectionParameter> Parameters { get; set; }
    }

    public class SectionParameter
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
