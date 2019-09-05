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
using uSlack.Configuration;
using Umbraco.Core.Logging;

namespace uSlack.Services
{
    public class SlackService : IMessageService
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
            return new SlackTaskClient(configuration.AppSettings.Token);
        }


        /// <summary>
        ///  It sends a messsage for each of the available configurations.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="evt"></param>        
        public virtual async Task SendMessageAsync(string service, string evt, IDictionary<string, string> properties = null)
        {
            foreach (var c in configuration.AppSettings.ConfigurationGroups)
            {
                if (c.GetParameter<bool>(service, evt) == false) return;

                var templateName = configuration.GetMessageTemplateName(service, evt);
                var msg = configuration.GetMessage(templateName);
                var blocksJsonwithPlaceholdersReplaced = JsonConvert.SerializeObject(msg.Blocks)
                                .ReplacePlaceholders(properties);

                var blocksArray = JsonConvert.DeserializeObject<Block[]>(blocksJsonwithPlaceholdersReplaced);

                var text = msg.Text.ReplacePlaceholders(properties);

                await this.SendMessageAsync(c.SlackChannel, text, blocksArray);
            }
        }


        private async Task SendMessageAsync(string channel, string txt, IBlock[] blocks)
        {
            if (string.IsNullOrWhiteSpace(channel))
            {
                throw new ArgumentException("Channel has not been set", nameof(channel));
            }

            if (string.IsNullOrWhiteSpace(txt))
            {
                throw new ArgumentException("Message text needs to be passed", nameof(txt));
            }

            if (blocks == null)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            try
            {
                var response = await client.Value.PostMessageAsync(channel, txt, blocks: blocks);

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
