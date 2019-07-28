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

            _messagingService.SendMessage("mediaService", "trashed", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        [EventHandler("saved", true)]
        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            _messagingService.SendMessage("mediaService", "saved", e.SavedEntities);
        }

        [EventHandler("moved", true)]
        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            _messagingService.SendMessage("mediaService", "moved", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        [EventHandler("deleted", true)]
        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            _messagingService.SendMessage("mediaService", "deleted", e.DeletedEntities);
        }

    }
}
