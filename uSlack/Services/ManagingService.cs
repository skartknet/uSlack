using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public class ManagingService
    {

        public Task<ConversationListResponse> GetChannelsAsync(string token)
        {
            var client = new USlackExtendedSlackTaskClient(token);
            return client.GetConversationListAsync();
        }
    }
}
