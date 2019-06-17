// <copyright file="SlackAPIExtensions.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public class USlackExendedSlackTaskClient : SlackTaskClient
    {
        public USlackExendedSlackTaskClient(string token)
            : base(token)
        { }

        public USlackExendedSlackTaskClient(string token, IWebProxy proxySettings)
            : base(token, proxySettings)
        { }

        public Task<PostMessageResponse> PostMessageOnlyBlocksAsync(
            string channelId,
            string text,
            string blocks
           )
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("channel", channelId),
                new Tuple<string, string>("text", text),
                new Tuple<string, string>("blocks", blocks)
            };

            return APIRequestWithTokenAsync<PostMessageResponse>(parameters.ToArray());
        }

        public Task<ConversationListResponse> GetConversationListAsync(bool ExcludeArchived = true)
        {
            return APIRequestWithTokenAsync<ConversationListResponse>(new Tuple<string, string>("exclude_archived", ExcludeArchived ? "1" : "0"));
        }
    }
}
