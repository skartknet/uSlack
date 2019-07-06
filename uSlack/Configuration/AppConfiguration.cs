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
    public class AppConfiguration
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; set; }

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
                var val = (T)this.Sections[section].Parameters[parameter];
                return (T)val;
            }
            catch (KeyNotFoundException ex)
            {
                return default(T);
            }
        }

        public ConfigSection AddSection(string alias, Action<ConfigSection> configSection)
        {
            return AddSection(new ConfigSection(alias, configSection));
        }

        public ConfigSection AddSection(ConfigSection section{
            Sections.Add(section.Alias, section);

            return section;
        })
    }
}
