using SlackAPI;
using System;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack
{
    public class SlackService : IMessageService
    {
        private readonly string _token;
        private readonly string _channel;


        public SlackService(IAppConfiguration config)
        {
            _token = config.Token;
            _channel = config.SlackChannel;
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
                    //TODO Log error
                    //Logger.("Message sending failed. error: " + response.error);
                }
            }
            catch (Exception ex)
            {
                //TODO Log Error
            }


        }
    }
}
