using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public class UslackConfiguration
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; set; }

        [JsonProperty("sections")]
        public List<ConfigSection> Sections { get; set; }
    }
}
