using SlackAPI;
using SlackAPI.Interactive;
using System.Threading.Tasks;

namespace uSlack.Services
{
    public class ManagingService
    {

        public Task<ConversationListResponse> GetChannelsAsync(string token)
        {
            var client = new SlackTaskClient(token);
            return client.GetConversationListAsync();
        }
    }
}
