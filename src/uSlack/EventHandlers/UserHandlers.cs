// <copyright file="UserHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using uSlack.Configuration;
using uSlack.Helpers;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    [SectionHandler("userService", "Umbraco User Service")]
    public class UserHandlers
    {
        private readonly IMessageService _messagingService;

        public UserHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        [EventHandler("deletedUser", "User deleted", true)]
        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("userService", "deletedUser", properties));
            }
        }

        [EventHandler("deletedUserGroup", "User gorup deleted", true)]
        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                var properties = new PropertiesDictionary(item);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("userService", "deletedUserGroup", properties));
            }
        }


    }
}
