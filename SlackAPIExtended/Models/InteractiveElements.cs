using System;
using Newtonsoft.Json;
using SlackAPI;

namespace SlackAPIExtended.Models
{
    public interface IAction
    {
        string ActionTs { get; set; }

        string BlockId { get; set; }

    }


public class ButtonElementInteractive : ButtonElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }
    }

    public class DatePickerElementInteractive : DatePickerElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_date")]
        public DateTime SelectedDate{ get; set; }
    }


    public class StaticSelectElementInteractive : StaticSelectElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_option")]
        public Option SelectedOption{ get; set; }
    }
    public class ExternalSelectElementInteractive : ExternalSelectElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }
        [JsonProperty("selected_option")]
        public Option SelectedOption{ get; set; }
    }


    public class UserSelectElementInteractive  : UserSelectElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_user")]
        public string SelectedUser { get; set; }
    }
    public class ConversationSelectElementInteractive  : ConversationSelectElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_conversation")]
        public string SelectedConversation { get; set; }
    }
    public class ChannelSelectElementInteractive  : ChannelSelectElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_channel")]
        public string SelectedChannel { get; set; }
    }
    public class OverflowElementInteractive : OverflowElement, IAction
    {
        [JsonProperty("action_ts")]
        public string ActionTs { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }
    }
}
