using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uSlack.EventHandlers
{
    public interface IContentHandlers
    {
        void ContentService_Published(IContentService sender, ContentPublishedEventArgs e);
        void ContentService_Unpublished(IContentService sender, PublishEventArgs<IContent> e);
        void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e);
        void ContentService_RolledBack(IContentService sender, RollbackEventArgs<IContent> e);
        void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e);
        void ContentService_Moved(IContentService sender, MoveEventArgs<IContent> e);
    }
}