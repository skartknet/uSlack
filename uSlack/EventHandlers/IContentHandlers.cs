using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace uSlack.EventHandlers
{
    public interface IContentHandlers
    {
        void ContentService_Published(IContentService sender, ContentPublishedEventArgs e);
    }
}