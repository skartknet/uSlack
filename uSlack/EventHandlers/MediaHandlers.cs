// <copyright file="MediaHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    [SectionHandler("mediaService")]
    public class MediaHandlers
    {
        private readonly IMessageService<IEntity> _messagingService;

        public MediaHandlers(IMessageService<IEntity> messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("trashed", true)]
        public void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                _messagingService.SendMessage("mediaService", "trashed", properties);
            }
        }

        [EventHandler("saved", true)]
        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.SavedEntities)
            {
                var properties = new PropertiesDictionary(item);
                _messagingService.SendMessage("mediaService", "saved", properties);
            }
        }

        [EventHandler("moved", true)]
        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                _messagingService.SendMessage("mediaService", "moved", properties);
            }
        }

        [EventHandler("deleted", true)]
        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);

                _messagingService.SendMessage("mediaService", "deleted", properties);
            }
        }

    }
}
