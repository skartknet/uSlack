using System;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public class MediaHandlers : EventHandlerBase
    {

        public MediaHandlers(IMessageService messageService,
                              IConfigurationService config) : base(messageService, config)
        { }


        public void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (MoveEventInfo<IMedia> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Media item has been trashed", nameof(this.MediaService_Trashed)));
            }
        }

        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.SavedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Media item has been saved", nameof(this.MediaService_Saved)));
            }
        }

        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (MoveEventInfo<IMedia> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Media item has been moved", nameof(this.MediaService_Moved)));
            }
        }

        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Media item has been deleted", nameof(this.MediaService_Deleted)));
            }
        }

    }
}
