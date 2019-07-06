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
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, bool> Parameters { get; set; }

        public ConfigSection(string alias, Action<ConfigSection> config)
        {
            Alias = alias;
            config?.Invoke(this);
        }


        public ConfigEvent SetEvent("alias", Action<ConfigEvent> config)
        {

        }

        public ConfigSection SetHandler(string alias, bool defaultValue)
        {
            Parameters.Add(alias, defaultValue);
            return this;
        }
    }


    public class ConfigEvent
    {
        private IEnumerable<Dictionary<string, Func<>> _handlers;

        public ConfigEvent SetHandler()
        {

        }
    }

}
