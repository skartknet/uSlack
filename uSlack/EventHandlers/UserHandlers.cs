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

        public UserHandlers(IConfigurationService config) : base(config)
        { }

        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "User has been deleted", nameof(this.UserService_DeletedUser)));
            }
        }

        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            foreach (IUserGroup item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "User group has been deleted", nameof(this.UserService_DeletedUserGroup)));
            }
        }


    }
}
