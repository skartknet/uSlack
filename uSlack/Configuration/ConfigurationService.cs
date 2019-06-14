// <copyright file="ConfigurationService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Umbraco.Core.IO;

namespace uSlack.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        const string _filesLocation = "~/App_Plugins/uSlack/Config/";

        public Dictionary<string, string> Messages { get; } = new Dictionary<string, string>();

        public UslackConfiguration AppConfiguration { get; } = new UslackConfiguration();

        public ConfigurationService Initialize()
        {
            InitializeConfiguration();
            InitializeMessages();

            return this;
        }

        private void InitializeConfiguration()
        {
            var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");

            if (File.Exists(msgPath))
            {
                var content = File.ReadAllText(msgPath);
                JsonConvert.PopulateObject(content, AppConfiguration);
            }

        }

        private void InitializeMessages()
        {
            var msgPath = IOHelper.MapPath(_filesLocation);
            if (Directory.Exists(msgPath) == false) return;
            var files = Directory.GetFiles(msgPath, "*.json");

            foreach (var file in files)
            {
                if (Messages.ContainsKey(file)) continue;
                var content = File.ReadAllText(file);
                Messages.Add(Path.GetFileNameWithoutExtension(file).ToUpperInvariant(), content);
            }
        }

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="model"></param>
        public void SaveAppConfiguration(UslackConfiguration model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");
            var json = JsonConvert.SerializeObject(model);

            File.WriteAllText(msgPath, json);
        }

        /// <summary>
        /// Gets a value from the configuration dictionary and casts it to the provided type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">parameter alias</param>
        /// <param name="section">section alias</param>
        /// <returns></returns>
        public T GetParameter<T>(string parameter, string section)
        {
            // warn: casting to int will give an error. Always cast to Int64;
            try
            {
                var val = (T)AppConfiguration.Sections[section].Parameters[parameter];
                return (T)val;
            }
            catch (KeyNotFoundException ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets a message body
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string GetMessage(string alias)
        {
            if (Messages.TryGetValue(alias.ToUpperInvariant(), out string message))
            {
                return message;
            }

            throw new FileNotFoundException($"Content for alias {alias} couldn't found");

        }
    }
}
