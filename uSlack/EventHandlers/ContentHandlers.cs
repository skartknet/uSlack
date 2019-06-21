﻿// <copyright file="ContentHandlers.cs" company="Mario Lopez">
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
    public class ContentHandlers : EventHandlerBase
    {


        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("published", "contentService") == false) return;

            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, nameof(this.ContentService_Published)));
            }
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("unpublished", "contentService") == false) return;

            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, nameof(this.ContentService_Unpublished)));
            }

        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("trashed", "contentService") == false) return;

            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, nameof(this.ContentService_Trashed)));
            }
        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("rolledBack", "contentService") == false) return;

            Task.Run(async () => await SendMessageAsync(e.Entity, nameof(this.ContentService_RolledBack)));
        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("deleted", "contentService") == false) return;

            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, nameof(this.ContentService_Deleted)));
            }

        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("moved", "contentService") == false) return;

            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, nameof(this.ContentService_Moved)));
            }
        }

        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (UslackConfiguration.Current.GetParameter<bool>("sentToPublish", "contentService") == false) return;

            Task.Run(async () => await SendMessageAsync(e.Entity, nameof(this.ContentService_Moved)));
        }


    }
}
