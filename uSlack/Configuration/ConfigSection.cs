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
        [JsonProperty("parameters")]
        public Dictionary<string, object> Parameters { get; set; }
    }

}
