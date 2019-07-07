// <copyright file="MediaHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public class MediaHandlers : EventHandlerBase
    {

        public MediaHandlers(IConfiguration config):base(config)
        {

        }

        public void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {

            SendMessage("mediaService", "trashed", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            SendMessage("mediaService", "saved", e.SavedEntities);
        }

        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            SendMessage("mediaService", "moved", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            SendMessage("mediaService", "deleted", e.DeletedEntities);
        }

    }
}
