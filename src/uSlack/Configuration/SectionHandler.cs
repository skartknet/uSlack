// <copyright file="ConfigSection.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;

namespace uSlack.Configuration
{
    public class SectionHandler
    {        
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}