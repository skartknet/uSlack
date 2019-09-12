// <copyright file="SlackService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlackAPI;
using SlackAPI.Composition;
using uSlack.Configuration;

namespace uSlack.Services
{
    public class SlackService : IMessageService
    {
        private readonly IContext configuration;
        private Lazy<SlackTaskClient> client;

        public SlackService(IContext configuration)
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
                var msgConfig = configuration.GetMessage(templateName);

                string msg = JsonConvert.SerializeObject(msgConfig.Blocks);
                var text = msgConfig.Text;

                if (properties != null)
                {
                    msg = msg.ReplacePlaceholders(properties);
                    text = text.ReplacePlaceholders(properties);
                }

                var blocksArray = JsonConvert.DeserializeObject<IBlock[]>(msg);

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

            var response = await client.Value.PostMessageAsync(channel, txt, blocks: blocks);

            if (!response.ok)
            {
                throw new FormatException("Error sending message to Slack. Code: " + response.error);
            }

        }

    }
}
