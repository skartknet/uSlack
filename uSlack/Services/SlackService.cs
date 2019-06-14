// <copyright file="SlackService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using uSlack.Configuration;
using uSlack.Extensions;
using uSlack.Services;

namespace uSlack.Services
{
    public class SlackService : IMessageService
    {
        private readonly string _token;
        private readonly string _channel;
        private readonly SlackTaskClient _slackClient;

        public SlackService(IConfigurationService config)
        {
            _token = config.AppConfiguration.Token;
            _channel = config.AppConfiguration.SlackChannel;
            _slackClient = new SlackTaskClient(_token);
        }
        public async Task SendMessageAsync(string txt, string blocks)
        {
            if (string.IsNullOrEmpty(blocks))
            {
                throw new ArgumentException("blocks cannot be empty", nameof(blocks));
            }

            try
            {
                var response = await _slackClient.PostMessageOnlyBlocksAsync(_channel, txt, blocks);
                
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

        public async Task Get()
        {
            try
            {
                var response = await _slackClient.GetChannelListAsync();
                
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
