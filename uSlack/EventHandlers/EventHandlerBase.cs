// <copyright file="EventHandlerBase.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;
using uSlack.Services;
using SlackAPI;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        protected readonly IMessageService _messageService;        

        protected EventHandlerBase()
        {            
            _messageService = new SlackService();
        }

        protected async Task SendMessageAsync(IEntity node, string templateName)
        {
            var msg = UslackConfiguration.Current.GetMessage(templateName);
            var blocksJsonwithPlaceholdersReplaced = JsonConvert.SerializeObject(msg.Blocks)
                            .ReplacePlaceholders(node);

            var blocks = JsonConvert.DeserializeObject<Block[]>(blocksJsonwithPlaceholdersReplaced);

            var text = msg.Text.ReplacePlaceholders(node);

            await _messageService.SendMessageAsync(text, blocks);
        }

    }
}
