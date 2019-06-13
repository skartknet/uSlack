using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Extensions;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        private readonly IMessageService _messageService;
        private readonly MessagesConfiguration _config;

        public EventHandlerBase(IMessageService messageService,
                                IConfigurationService config)
        {
            _messageService = messageService;
            _config = config.MessagesConfiguration;
        }

        public async Task SendMessageAsync(IEntity node, string subject, string templateName)
        {
            var json = _config.GetMessage(templateName);
            var txtReplaced = json.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(subject, txtReplaced);
        }

    }
}
