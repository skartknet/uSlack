using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack
{
    public static class SlackAPIExtensions
    {
        public static Task<PostMessageResponse> PostMessageOnlyBlocksAsync(
            this SlackTaskClient client,
            string channelId,
            string text,
            string blocks
           )
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));
            parameters.Add(new Tuple<string, string>("blocks", blocks));

            return client.APIRequestWithTokenAsync<PostMessageResponse>(parameters.ToArray());
        }
    }
}
