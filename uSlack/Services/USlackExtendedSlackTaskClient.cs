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
    public class USlackExtendedSlackTaskClient : SlackTaskClient
    {
        public USlackExtendedSlackTaskClient(string token)
            : base(token)
        { }

        public USlackExtendedSlackTaskClient(string token, IWebProxy proxySettings)
            : base(token, proxySettings)
        { }

        public async Task<PostMessageResponse> PostMessageOnlyBlocksAsync(
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

            return await APIRequestWithTokenAsync<PostMessageResponse>(parameters.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExcludeArchived"></param>
        /// <returns></returns>
        internal async Task<ConversationListResponse> GetConversationListAsync(bool ExcludeArchived = true)
        {
            try
            {
                var results = await APIRequestWithTokenAsync<ConversationListResponse>(new Tuple<string, string>("exclude_archived", ExcludeArchived ? "1" : "0"));
                return results;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
