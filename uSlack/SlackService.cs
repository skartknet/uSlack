using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack
{
    public class SlackService
    {
        public async Task SendMessage()
        {
            var token = "xoxp-656657692176-645232876739-658179266944-834090019227aa80b4a9f33d43f615ab";
            var client = new SlackTaskClient(token);
            var channel = "uslack";
            var text = "Content Created!";

            var response = await client.PostMessageAsync(channel, text);

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
