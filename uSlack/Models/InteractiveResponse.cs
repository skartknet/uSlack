using Newtonsoft.Json;
using SlackAPI;

namespace uSlack.Models
{
    using uSlack.Models;

    public class InteractiveResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("trigger_id")]
        public string TriggerId { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("actions")]
        public Action[] Actions { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }
    }

   

}
