using System.Threading.Tasks;
using Umbraco.Core.Models;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        private readonly IMessageService _messageService;
        private readonly MessagesConfiguration _config;

        public EventHandlerBase(IMessageService messageService,
                                IAppConfiguration config)
        {
            _messageService = messageService;
            _config = config.Messages;
        }

        public async Task SendMessageAsync(IContent node, string subject, string templateName)
        {
            var json = _config.GetMessage(templateName);
            var txtReplaced = json.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(subject, txtReplaced);
        }

        public async Task SendMessageAsync(IMedia node, string subject, string templateName)
        {
            var json = _config.GetMessage(templateName);
            var txtReplaced = json.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(subject, txtReplaced);
        }
    }
}
