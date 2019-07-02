// <copyright file="SlackService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using uSlack.Configuration;
using uSlack.Services;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public class SlackService : IMessageService
    {

        public async Task SendMessageAsync(string token, string channel, string txt, IBlock[] blocks)
        {
            if (blocks == null)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            try
            {
                var client = new USlackExendedSlackTaskClient(token);
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

        /// <summary>
        /// Get all Slack conversations
        /// </summary>
        /// <param name="token">If no token is passed, the on in the config will be used.</param>
        /// <returns></returns>
        public async Task<ConversationListResponse> GetChannelsAsync(string token)
        {            
            var client = new USlackExendedSlackTaskClient(token);

            var response = await client.GetConversationListAsync();

            if (!response.ok)
            {
                Current.Logger.Error(typeof(SlackService), "Error sending message to Slack. Response: {Response}", response.error);
            }

            return response;
        }

        
    }
}
