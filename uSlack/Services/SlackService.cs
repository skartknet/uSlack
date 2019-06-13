using SlackAPI;
using System;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using uSlack.Configuration;
using uSlack.Extensions;
using uSlack.Services;

namespace uSlack.Services
{
    public class SlackService : IMessageService
    {
        private readonly string _token;
        private readonly string _channel;


        public SlackService(IConfigurationService config)
        {
            _token = config.AppConfiguration.Token;
            _channel = config.AppConfiguration.SlackChannel;
        }
        public async Task SendMessageAsync(string txt, string blocks)
        {
            if (string.IsNullOrEmpty(blocks))
            {
                throw new ArgumentException("blocks cannot be empty", nameof(blocks));
            }

            var client = new SlackTaskClient(_token);

            try
            {
                var response = await client.PostMessageOnlyBlocksAsync(_channel, txt, blocks);

                // process response from API call
                if (!response.ok)
                {                    
                    Current.Logger.Error(typeof(SlackService), "Error sending message to Slack. Response: {Response}", response.error);
                }
            }
            catch (Exception ex)
            {
                Current.Logger.Error(typeof(SlackService), ex);
            }

        }
    }
}
