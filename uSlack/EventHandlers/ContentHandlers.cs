// <copyright file="ContentHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{

    [SectionHandler("contentService")]
    public class ContentHandlers 
    {
        private readonly IMessageService<IEntity> _messagingService;

        public ContentHandlers(IMessageService<IEntity> messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("published", true)]
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            _messagingService.SendMessage("contentService", "published", e.PublishedEntities);
        }

        [EventHandler("unpublished", true)]
        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "unpublished", e.PublishedEntities);
        }

        [EventHandler("trashed", true)]
        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "trashed", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        [EventHandler("rolledBack", true)]
        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "rolledBack", e.Entity);
        }

        [EventHandler("deleted", true)]
        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "deleted", e.DeletedEntities);
        }

        [EventHandler("moved", true)]
        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "moved", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        [EventHandler("sentToPublish", true)]
        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "sentToPublish", e.Entity);
        }


    }
}
