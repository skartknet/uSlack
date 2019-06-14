// <copyright file="SlackAPIExtensions.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace uSlack.Extensions
{
    public static class SlackAPIExtensions
    {
        public static Task<PostMessageResponse> PostMessageOnlyBlocksAsync(
            this SlackTaskClient client,
            string channelId,
            string text,
            string blocks
           )
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));
            parameters.Add(new Tuple<string, string>("blocks", blocks));

            return client.APIRequestWithTokenAsync<PostMessageResponse>(parameters.ToArray());
        }
    }
}
