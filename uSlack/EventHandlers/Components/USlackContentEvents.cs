// https://our.umbraco.com/Documentation/implementation/composing/

using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace uSlack.EventHandlers.Components
{
    public class USlackContentEvents : IComponent
    {
        private readonly IContentHandlers _handlers;

        public USlackContentEvents(IContentHandlers handlers)
        {
            _handlers = handlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            ContentService.Published += _handlers.ContentService_Published;
        }

       

        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class USlackContentComposer : ComponentComposer<USlackContentEvents>
    {
        // nothing needed to be done here!
    }
}