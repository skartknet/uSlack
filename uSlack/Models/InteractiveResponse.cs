﻿using System.Collections.Generic;
using Newtonsoft.Json;
using uSlack.Deserialisation;

namespace uSlack.Models
{
    // https://api.slack.com/messaging/interactivity/enabling (3. Prepare to receive request payloads)
    public class InteractiveResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("trigger_id")]
        public string TriggerId { get; set; }

        [JsonProperty("channel")]
        public Channel Channel { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        [JsonProperty("actions")]
        [JsonConverter(typeof(InteractiveActionConverter))]
        public IList<IAction> Actions { get; set; }
    }



}
