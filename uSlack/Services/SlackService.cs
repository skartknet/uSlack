// <copyright file="SlackService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using SlackAPI;
using SlackAPI.Interactive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.Entities;
using uSlack.Configuration;

namespace uSlack.Services
{
    public class SlackService : IMessageService<IEntity>
    {
        private readonly IConfiguration configuration;

        private Lazy<SlackTaskClient> client;

        public SlackService(IConfiguration configuration)
        {
            this.configuration = configuration;
            client = new Lazy<SlackTaskClient>(InitSlackClient);
        }

        private SlackTaskClient InitSlackClient()
        {
            return new SlackTaskClient()
        }



        /// <summary>
        /// Get all Slack conversations
        /// </summary>
        /// <param name="token">If no token is passed, the one in the config will be used.</param>
        /// <returns></returns>
        public async Task<ConversationListResponse> GetChannelsAsync(string token)
        {            
            var client = new SlackTaskClient();

            var response = await client.GetConversationListAsync();

            if (!response.ok)
            {
                Current.Logger.Error(typeof(SlackService), "Error sending message to Slack. Response: {Response}", response.error);
            }

            return response;
        }


        /// <summary>
        ///  It sends a messsage for each of the available configurations.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="evt"></param>        
        public virtual async Task SendMessage(string service, string evt, IEntity entity)
        {
            foreach (var c in configuration.Groups)
            {
                if (c.GetParameter<bool>(service, evt) == false) return;

                await SendMessageAsync(c.Token, c.SlackChannel, entity, $"{service}_{evt}");

            }
        }

        /// <summary>
        ///  It sends a messsage for each of the available configurations.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="evt"></param>        
        public virtual void SendMessage(string service, string evt, IEnumerable<IEntity> entities)
        {
            foreach (var c in configuration.Groups)
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
            
            await this.SendMessageAsync(token, channel, text, blocks);
        }

        private async Task SendMessageAsync(string token, string channel, string txt, IBlock[] blocks)
        {
            if (blocks == null)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            try
            {
                var client = new SlackTaskClient(token);
                var response = await client.PostMessageAsync(channel, txt, blocks: blocks);

                if (!response.ok)
                {
                    Current.Logger.Error(typeof(SlackService), "Error sending message to Slack. Response: {Response}", response.error);
                }
            }
            catch (Exception ex)
            {
                Current.Logger.Warn(typeof(SlackService), ex, "Error sending uSlack message");
            }

        }

    }
}
