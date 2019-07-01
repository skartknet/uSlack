// <copyright file="ContentHandlers.cs" company="Mario Lopez">
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


    public class ContentHandlers : EventHandlerBase
    {

        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            SendMessage("contentService", "published", e.PublishedEntities);
        }

        public void ContentService_Unpublished(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "unpublished", e.PublishedEntities);
        }

        public void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "trashed", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void ContentService_RolledBack(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "rolledBack", e.Entity);
        }

        public void ContentService_Deleted(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "rolledBack", e.DeletedEntities);
        }

        public void ContentService_Moved(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "moved", e.MoveInfoCollection.Select(mi => mi.Entity));
        }

        public void ContentService_SentToPublish(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.SendToPublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            SendMessage("contentService", "sentToPublish", e.Entity);
        }


    }
}
