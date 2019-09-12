// <copyright file="MediaHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using uSlack.Configuration;
using uSlack.Helpers;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    [SectionHandler("mediaService", "Umbraco Media Service")]
    public class MediaHandlers
    {
        private readonly IMessageService _messagingService;

        public MediaHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("trashed", "Media item trashed", true)]
        public void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("mediaService", "trashed", properties));
            }
        }

        [EventHandler("saved", "Media item saved", true)]
        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.SavedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("mediaService", "saved", properties));
            }
        }

        [EventHandler("moved", "Media item moved", true)]
        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("mediaService", "moved", properties));
            }
        }

        [EventHandler("deleted", "Media item deleted", true)]
        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("mediaService", "deleted", properties));
            }
        }

    }
}
