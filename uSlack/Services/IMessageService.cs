// <copyright file="IMessageService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public interface IMessageService<T> where T : class
    {
        Task SendMessageAsync(string token, string channel, string txt, IBlock[] blocks);
        Task<ConversationListResponse> GetChannelsAsync(string token);
        void SendMessage(string service, string evt, T entity);
        void SendMessage(string service, string evt, IEnumerable<T> entities);
        Task SendMessageAsync(string token, string channel, T node, string templateName);
    }
}