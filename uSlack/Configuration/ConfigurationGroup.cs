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
    public class ConfigurationGroup
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; set; }

        [JsonProperty("groups")]
        public IEnumerable<string> Groups { get; set; }

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
