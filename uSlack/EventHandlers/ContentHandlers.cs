// <copyright file="ContentHandlers.cs" company="Mario Lopez">
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

        public ContentHandlers(IConfigurationService config) : base(config)
        { }
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            if (_config.GetParameter<bool>("published", "contentService") == false) return;

            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been published", nameof(this.ContentService_Published)));
            }
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (_config.GetParameter<bool>("unpublished", "contentService") == false) return;

            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been unpublished", nameof(this.ContentService_Unpublished)));
            }

        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (_config.GetParameter<bool>("trashed", "contentService") == false) return;

            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Content item has been trashed", nameof(this.ContentService_Trashed)));
            }
        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (_config.GetParameter<bool>("rolledBack", "contentService") == false) return;

            Task.Run(async () => await SendMessageAsync(e.Entity, "Content item has been rolledback", nameof(this.ContentService_RolledBack)));
        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (_config.GetParameter<bool>("deleted", "contentService") == false) return;

            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been deleted", nameof(this.ContentService_Deleted)));
            }

        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (_config.GetParameter<bool>("moved", "contentService") == false) return;

            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Content item has been moved", nameof(this.ContentService_Moved)));
            }
        }

    }
}
