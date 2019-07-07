// <copyright file="EventHandlerBase.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;
using uSlack.Services;
using SlackAPI;
using System.Collections.Generic;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public abstract class EventHandlerBase
    {
        private readonly IConfiguration configuration;

        public EventHandlerBase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

           /// <summary>
        ///  It sends a messsage for each of the available configurations.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="evt"></param>        
        protected virtual void SendMessage(string service, string evt, IEntity entity)
        {
            foreach (var c in configuration.AppConfiguration)
            {
                if (c.GetParameter<bool>(service, evt) == false) return;

                Task.Run(async () => await SendMessageAsync(c.Token, c.SlackChannel, entity, $"{service}_{evt}"));

            }
        }

        /// <summary>
        ///  It sends a messsage for each of the available configurations.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="evt"></param>        
        protected virtual void SendMessage(string service, string evt, IEnumerable<IEntity> entities)
        {
            foreach (var c in configuration.AppConfiguration)
            {
                if (c.GetParameter<bool>(service, evt) == false) return;

                foreach (var entity in entities)
                {
                    Task.Run(async () => await SendMessageAsync(c.Token, c.SlackChannel, entity, $"{service}_{evt}"));
                }

            }
        }


        private async Task SendMessageAsync(string token, string channel, IEntity node, string templateName)
        {
            var msg = configuration.GetMessage(templateName);
            var blocksJsonwithPlaceholdersReplaced = JsonConvert.SerializeObject(msg.Blocks)
                            .ReplacePlaceholders(node);

            var blocks = JsonConvert.DeserializeObject<Block[]>(blocksJsonwithPlaceholdersReplaced);

            var text = msg.Text.ReplacePlaceholders(node);

            //TODO: get from DI container
            var messageService = new SlackService();
            await messageService.SendMessageAsync(token, channel, text, blocks);
        }


      
    }
}
