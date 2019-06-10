// https://our.umbraco.com/Documentation/implementation/composing/

using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace uSlack.EventHandlers.Components
{
    public class MediaServiceEvents : IComponent
    {
        private readonly MediaHandlers _handlers;

        public MediaServiceEvents(MediaHandlers handlers)
        {
            _handlers = handlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            MediaService.Deleted += _handlers.MediaService_Deleted;
            MediaService.EmptiedRecycleBin += _handlers.MediaService_EmptiedRecycleBin;
            MediaService.Moved += _handlers.MediaService_Moved;
            MediaService.Saved += _handlers.MediaService_Saved;
            MediaService.Trashed += _handlers.MediaService_Trashed;            
        }


        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class USlackMediaServiceComposer : ComponentComposer<MediaServiceEvents>
    {
        // nothing needed to be done here!
    }
}