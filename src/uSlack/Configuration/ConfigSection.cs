﻿// <copyright file="ConfigSection.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System.Collections.Generic;

namespace uSlack.Configuration
{
    public class ConfigSection
    {        
        [JsonProperty("label")]        
        public string Label { get; set; }

        [JsonProperty("handlers")]
        public Dictionary<string, SectionHandler> SectionHandlers { get; set; } = new Dictionary<string, SectionHandler>();
    }

}
