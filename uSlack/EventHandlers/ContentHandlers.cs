// <copyright file="ContentHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{

    [SectionHandler("contentService")]
    public class ContentHandlers : EventHandlerBase
    {
        private readonly IMessageService _messagingService;

        public ContentHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("published", true)]
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            _messagingService.SendMessage("contentService", "published", e.PublishedEntities);
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "unpublished", e.PublishedEntities);
        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "trashed", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "rolledBack", e.Entity);
        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "rolledBack", e.DeletedEntities);
        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "moved", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            _messagingService.SendMessage("contentService", "sentToPublish", e.Entity);
        }


    }
}
