﻿// <copyright file="UserHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public class UserHandlers : EventHandlerBase
    {
        private readonly IMessageService _messagingService;

        public UserHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }
        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            _messagingService.SendMessage("userService", "deletedUser", e.DeletedEntities);
        }

        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            _messagingService.SendMessage("userService", "deletedUserGroup", e.DeletedEntities);
        }


    }
}
