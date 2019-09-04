// <copyright file="UslackConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace uSlack.Configuration
{

    public class AppSettings
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("configurationGroups")]
        public IEnumerable<ConfigurationGroup> ConfigurationGroups { get; set; }
    }

    public class ConfigurationGroup
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; set; }

        [JsonProperty("securityGroups")]
        public IEnumerable<string> SecurityGroups { get; set; }

        [JsonProperty("sections")]
        public Dictionary<string, ConfigSection> Sections { get; set; }

        /// <summary>
        /// Gets a value from the configuration dictionary and casts it to the provided type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">section alias</param>
        /// <param name="parameter">parameter alias</param>
        /// <returns></returns>
        public T GetParameter<T>(string section, string parameter) where T : struct
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                throw new ArgumentException("section cannot be empty", nameof(section));
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException("parameter cannot be empty", nameof(parameter));
            }

            try
            {
                var val = (T)this.Sections[section].SectionHandlers[parameter];
                return (T)val;
            }
            catch (KeyNotFoundException ex)
            {
                return default(T);
            }
        }
    }
}
