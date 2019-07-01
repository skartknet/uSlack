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
    public class AppConfig
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
        public T GetParameter<T>(string section, string parameter)
        {
            // warn: casting to int will give an error. Always cast to Int64;
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
    }
}
