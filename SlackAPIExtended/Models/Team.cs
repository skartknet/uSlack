using Newtonsoft.Json;

namespace SlackAPIExtended.Models
{
    public class Team
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }
    }
}
