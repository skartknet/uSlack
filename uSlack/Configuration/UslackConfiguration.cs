// <copyright file="UslackConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public class AppConfigurationList : List<AppConfig>
    {
    }

    public class AppConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; set; }

        [JsonProperty("sections")]
        public Dictionary<string, ConfigSection> Sections { get; set; }
    }
}
