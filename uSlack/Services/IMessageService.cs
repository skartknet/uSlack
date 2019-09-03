// <copyright file="IMessageService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace uSlack.Services
{
    public interface IMessageService<T> where T : class
    {
        Task<SlackAPI.Interactive.ConversationListResponse> GetChannelsAsync(string token);
        Task SendMessage(string service, string evt, IDictionary<string, string> properties);        
    }
}