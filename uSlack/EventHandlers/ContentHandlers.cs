﻿using System;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class ContentHandlers : EventHandlerBase
    {

        public ContentHandlers(IMessageService messageService,
                              IConfigurationService config) : base(messageService, config)
        { }
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been published", nameof(this.ContentService_Published)));
            }
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.PublishedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been unpublished", nameof(this.ContentService_Unpublished)));
            }

        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Content item has been trashed", nameof(this.ContentService_Trashed)));
            }
        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {

            Task.Run(async () => await SendMessageAsync(e.Entity, "Content item has been rolledback", nameof(this.ContentService_RolledBack)));
        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Content item has been deleted", nameof(this.ContentService_Deleted)));
            }

        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (MoveEventInfo<IContent> info in e.MoveInfoCollection)
            {
                Task.Run(async () => await SendMessageAsync(info.Entity, "Content item has been moved", nameof(this.ContentService_Moved)));
            }
        }

    }
}
