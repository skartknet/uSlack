// <copyright file="ConversationListResult.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Services.Models
{
    [RequestPath("conversations.list")]

    public class ConversationListResponse : Response
    {
        public Channel[] channels;
    }
}
