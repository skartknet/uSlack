// <copyright file="IMessageService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System.Threading.Tasks;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string token, string channel, string txt, IBlock[] blocks);
        Task<ConversationListResponse> GetChannelsAsync(string token = null);
        void SendMessage(string service, string evt, Umbraco.Core.Models.Entities.IEntity entity);
        void SendMessage(string service, string evt, System.Collections.Generic.IEnumerable<Umbraco.Core.Models.Entities.IEntity> entities);
        Task SendMessageAsync(string token, string channel, Umbraco.Core.Models.Entities.IEntity node, string templateName);
    }
}