// <copyright file="ConfigSection.cs" company="Mario Lopez">
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
    public class ConfigSection
    {
        [JsonProperty("handlers")]
        public Dictionary<string, object> SectionHandlers { get; set; }
    }

}
