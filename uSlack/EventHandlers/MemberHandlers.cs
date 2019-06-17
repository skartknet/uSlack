﻿// <copyright file="MemberHandlers.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public class MemberHandlers : EventHandlerBase
    {

        public MemberHandlers(IConfigurationService config) : base(config)
        { }

        public void MemberService_Deleted(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMember> e)
        {
            if (_config.GetParameter<bool>("deleted", "memberService") == false) return;

            foreach (IMember item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Member has been deleted", nameof(this.MemberService_Deleted)));
            }
        }

        public void MemberService_Saved(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMember> e)
        {
            if (_config.GetParameter<bool>("saved", "memberService") == false) return;

            foreach (IMember item in e.SavedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Member has been saved", nameof(this.MemberService_Deleted)));
            }
        }

    }
}