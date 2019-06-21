// <copyright file="MessageConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public class MessageConfiguration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("blocks")]
        public Block[] Blocks { get; set; }
    }
}
