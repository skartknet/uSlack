using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be empty", nameof(message));
            }

            var client = new SlackTaskClient(_token);
         

            var response = await client.PostMessageAsync(_channel, message);

            // process response from API call
            if (response.ok)
            {
                Console.WriteLine("Message sent successfully");
            }
            else
            {
                Console.WriteLine("Message sending failed. error: " + response.error);
            }
        }
    }
}
