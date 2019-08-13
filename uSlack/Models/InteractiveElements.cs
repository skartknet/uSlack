using System;
using Newtonsoft.Json;
using SlackAPI;

namespace uSlack.Models
{
    public interface IAction
    {

        string action_id { get; set; }

        string BlockId { get; set; }

    }


    public class ButtonElementInteractive : ButtonElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }
    }

    public class DatePickerElementInteractive : DatePickerElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_date")]
        public DateTime SelectedDate { get; set; }
    }


    public class StaticSelectElementInteractive : StaticSelectElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_option")]
        public Option SelectedOption { get; set; }
    }
    public class ExternalSelectElementInteractive : ExternalSelectElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }
        [JsonProperty("selected_option")]
        public Option SelectedOption { get; set; }
    }


    public class UserSelectElementInteractive : UserSelectElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_user")]
        public string SelectedUser { get; set; }
    }
    public class ConversationSelectElementInteractive : ConversationSelectElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_conversation")]
        public string SelectedConversation { get; set; }
    }
    public class ChannelSelectElementInteractive : ChannelSelectElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("selected_channel")]
        public string SelectedChannel { get; set; }
    }
    public class OverflowElementInteractive : OverflowElement, IAction
    {


        [JsonProperty("block_id")]
        public string BlockId { get; set; }
    }
}
