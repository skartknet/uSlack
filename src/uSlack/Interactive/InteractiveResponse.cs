using Newtonsoft.Json;
using System.Collections.Generic;
using uSlack.Deserialisation;
using InteractiveModels = SlackAPI.Interactive;

namespace uSlack.Interactive
{
    // https://api.slack.com/messaging/interactivity/enabling (3. Prepare to receive request payloads)
    public class InteractiveResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("team")]
        public InteractiveModels.Team Team { get; set; }

        [JsonProperty("user")]
        public InteractiveModels.User User { get; set; }

        [JsonProperty("trigger_id")]
        public string TriggerId { get; set; }

        [JsonProperty("channel")]
        public InteractiveModels.Channel Channel { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        [JsonProperty("actions")]
        [JsonConverter(typeof(InteractiveActionConverter))]
        public IList<InteractiveModels.IInteractiveElement> Actions { get; set; }
    }



}
