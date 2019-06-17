// <copyright file="EventHandlerBase.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        protected readonly IMessageService _messageService;        

        protected EventHandlerBase()
        {            
            _messageService = new SlackService();
        }

        protected async Task SendMessageAsync(IEntity node, string subject, string templateName)
        {
            var json = UslackConfiguration.Current.GetMessage(templateName);
            var txtReplaced = json.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(subject, txtReplaced);
        }

    }
}
