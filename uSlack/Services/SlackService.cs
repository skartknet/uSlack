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

namespace uSlack.Services
{
    public class SlackService : IMessageService
    {

        private readonly IConfigurationService _config;

        public SlackService(IConfigurationService config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task SendMessageAsync(string txt, string blocks)
        {
            if (string.IsNullOrEmpty(blocks))
            {
                throw new ArgumentException("blocks cannot be empty", nameof(blocks));
            }

            try
            {
                var client = new USlackExendedSlackTaskClient(_config.AppConfiguration.Token);
                var response = await client.PostMessageOnlyBlocksAsync(_config.AppConfiguration.SlackChannel, txt, blocks);
                
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


        public async Task GetConversationsAsync()
        {
            try
            {
                var client = new USlackExendedSlackTaskClient(_config.AppConfiguration.Token);

                var response = await client.GetConversationListAsync();
                
                if (!response.ok)
                {
                    Current.Logger.Error(typeof(SlackService), "Error sending message to Slack. Response: {Response}", response.error);
                }
            }
            catch (Exception ex)
            {
                Current.Logger.Error(typeof(SlackService), ex);
            }
        }
    }
}
