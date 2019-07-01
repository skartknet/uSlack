// <copyright file="MemberHandlers.cs" company="Mario Lopez">
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

   

        public void MemberService_Deleted(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMember> e)
        {
            SendMessage("memberService", "deleted", e.DeletedEntities);
        }

        public void MemberService_Saved(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMember> e)
        {
            SendMessage("memberService", "saved", e.SavedEntities);
        }

    }
}
