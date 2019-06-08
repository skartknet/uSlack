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

        public SlackService(AppConfiguration config)
        {
            _token = config.Token;
        }
        public async Task SendMessageAsync(string message)
        {
            
            var client = new SlackTaskClient(_token);
            var channel = "uslack";
         

            var response = await client.PostMessageAsync(channel, message);

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
