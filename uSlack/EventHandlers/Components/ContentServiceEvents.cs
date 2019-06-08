// https://our.umbraco.com/Documentation/implementation/composing/

using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace uSlack.EventHandlers.Components
{
    public class ContentServiceEvents : IComponent
    {
        private readonly IContentHandlers _handlers;

        public ContentServiceEvents(IContentHandlers handlers)
        {
            _handlers = handlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            ContentService.Published += _handlers.ContentService_Published;
            ContentService.Unpublished += _handlers.ContentService_Unpublished;
            ContentService.Trashed += _handlers.ContentService_Trashed;
            ContentService.RolledBack += _handlers.ContentService_RolledBack;
            ContentService.Deleted += _handlers.ContentService_Deleted;
            ContentService.Moved += _handlers.ContentService_Moved;
        }

        










        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class USlackContentComposer : ComponentComposer<ContentServiceEvents>
    {
        // nothing needed to be done here!
    }
}