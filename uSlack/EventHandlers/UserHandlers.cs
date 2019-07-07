// <copyright file="UserHandlers.cs" company="Mario Lopez">
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

        public UserHandlers(IConfiguration config) : base(config)
        {

        }
        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            SendMessage("userService", "deletedUser", e.DeletedEntities);
        }

        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            SendMessage("userService", "deletedUserGroup", e.DeletedEntities);
        }


    }
}
