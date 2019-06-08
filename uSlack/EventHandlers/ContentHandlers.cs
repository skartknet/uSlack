using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class ContentHandlers : IContentHandlers
    {
        private readonly IMessageService _messageService;
        private readonly AppConfiguration _config;

        public ContentHandlers(IMessageService messageService,
                                AppConfiguration config)
        {
            _messageService = messageService;
            _config = config;
        }
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            var json = _config.Messages.GetMessage(nameof(this.ContentService_Published));
            _messageService.SendMessageAsync(json);
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            throw new NotImplementedException();

        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            throw new NotImplementedException();

        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            throw new NotImplementedException();

        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            throw new NotImplementedException();

        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            throw new NotImplementedException();
        }

    }
}
