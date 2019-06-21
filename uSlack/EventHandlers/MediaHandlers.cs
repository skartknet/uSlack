﻿// <copyright file="MediaHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

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

        public void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {


            foreach (var c in UslackConfiguration.Current.AppConfiguration)
            {
                if (c.GetParameter<bool>("trashed", "mediaService") == false) return;

                foreach (MoveEventInfo<IMedia> info in e.MoveInfoCollection)

                {
                    Task.Run(async () => await SendMessageAsync(info.Entity, nameof(this.MediaService_Trashed)));
                }
            }
        }

        public void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
         

            foreach (var c in UslackConfiguration.Current.AppConfiguration)
            {
                if (c.GetParameter<bool>("saved", "mediaService") == false) return;

                foreach (var item in e.SavedEntities)

                {
                    Task.Run(async () => await SendMessageAsync(item, nameof(this.MediaService_Saved)));
                }
            }
        }

        public void MediaService_Moved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("moved", "mediaService") == false) return;

            foreach (MoveEventInfo<IMedia> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, nameof(this.MediaService_Moved)));
            }


            foreach (var c in UslackConfiguration.Current.AppConfiguration)
            {
                if (c.GetParameter<bool>("saved", "mediaService") == false) return;

                foreach (var info in e.MoveInfoCollection)

                {
                    Task.Run(async () => await SendMessageAsync(info.Entity, nameof(this.MediaService_Saved)));
                }
            }
        }

        public void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("deleted", "mediaService") == false) return;

            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, nameof(this.MediaService_Deleted)));
            }
        }

    }
}
