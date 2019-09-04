// <copyright file="MemberHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    [SectionHandler("memberService")]
    public class MemberHandlers
    {
        private readonly IMessageService<IEntity> _messagingService;

        public MemberHandlers(IMessageService<IEntity> messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("deleted", true)]
        public void MemberService_Deleted(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMember> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);

                _messagingService.SendMessage("memberService", "deleted", properties);
            }
        }

        [EventHandler("saved", true)]
        public void MemberService_Saved(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMember> e)
        {
            foreach (var item in e.SavedEntities)
            {
                var properties = new PropertiesDictionary(item);

                _messagingService.SendMessage("memberService", "saved", properties);
            }
        }

    }
}
