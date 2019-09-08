// <copyright file="MessageConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using SlackAPI.Composition;

namespace uSlack.Configuration
{
    public class MessageConfiguration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("blocks")]
        public IBlock[] Blocks { get; set; }
    }
}
