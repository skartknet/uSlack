// <copyright file="EventHandlerBase.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;
using uSlack.Extensions;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        protected readonly IMessageService _messageService;
        protected readonly IConfigurationService _config;

        protected EventHandlerBase(IMessageService messageService,
                                IConfigurationService config)
        {
            _messageService = messageService;
            _config = config;
        }

        protected async Task SendMessageAsync(IEntity node, string subject, string templateName)
        {
            var json = _config.GetMessage(templateName);
            var txtReplaced = json.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(subject, txtReplaced);
        }

    }
}
