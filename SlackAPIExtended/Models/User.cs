using Newtonsoft.Json;

namespace SlackAPIExtended.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }
    }
}
