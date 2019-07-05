using Newtonsoft.Json;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Models
{
    public class InteractiveResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("trigger_id")]
        public string TriggerId { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("actions")]
        public Action[] Actions { get; set; }
    }

    public class Action : Element
    {

        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

    }

    

}
