// <copyright file="ContentHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Helpers;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{

    [SectionHandler("contentService")]
    public class ContentHandlers
    {
        private readonly IMessageService _messagingService;

        public ContentHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("published", true)]
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            foreach (var item in e.PublishedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "published", properties));
            }
        }

        [EventHandler("unpublished", true)]
        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.PublishedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "unpublished", properties));
            }
        }

        [EventHandler("trashed", true)]
        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "trashed", properties));
            }
        }

        [EventHandler("rolledBack", true)]
        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {

            var properties = new PropertiesDictionary(e.Entity);

            AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "rolledBack", properties));

        }

        [EventHandler("deleted", true)]
        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "deleted", properties));
            }
        }

        [EventHandler("moved", true)]
        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "moved", properties));
            }
        }

        [EventHandler("sentToPublish", true)]
        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var properties = new PropertiesDictionary(e.Entity);

            AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "sentToPublish", properties));
        }


    }
}
