// <copyright file="ContentHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq;
using Umbraco.Core.Composing;
using uSlack.Configuration;
using uSlack.Helpers;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{

    [SectionHandler("contentService", "Umbraco Content Service")]
    public class ContentHandlers
    {
        private readonly IMessageService _messagingService;

        public ContentHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("published", "Content item published", true)]
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            foreach (var item in e.PublishedEntities)
            {
                var properties = new PropertiesDictionary(item);
                var publisher = Current.Services.UserService.GetUserById(item.PublisherId.GetValueOrDefault());
                if (publisher != null) properties.Add("publisher", publisher.Name);   
                                
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "published", properties, publisher));
            }
        }

        [EventHandler("unpublished", "Content item unpublished", true)]
        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.PublishedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "unpublished", properties));
            }
        }

        [EventHandler("trashed", "Content item trashed", true)]
        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "trashed", properties));
            }
        }

        [EventHandler("rolledBack", "Content item rolled back", true)]
        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {

            var properties = new PropertiesDictionary(e.Entity);

            AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "rolledBack", properties));

        }

        [EventHandler("deleted", "Content item deleted", true)]
        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);
                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "deleted", properties));
            }
        }

        [EventHandler("moved", "Content item moved", true)]
        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.MoveInfoCollection.Select(mi => mi.Entity))
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "moved", properties));
            }
        }

        [EventHandler("sentToPublish", "Content item sent to publish", true)]
        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var properties = new PropertiesDictionary(e.Entity);

            AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "sentToPublish", properties));
        }


    }
}
