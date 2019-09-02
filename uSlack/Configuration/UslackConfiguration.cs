// <copyright file="ConfigurationService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.IO;

namespace uSlack.Configuration
{
    public class UslackConfiguration : IConfiguration
    {
        const string FilesLocation = "~/App_Plugins/uSlack/Config/";
        private readonly Lazy<IReadOnlyDictionary<string, MessageConfiguration>> _messages;
        private Lazy<IEnumerable<ConfigurationGroup>> _appConfiguration;
        private readonly Lazy<ConfigurationGroup> _defaultConfiguration;        

        public UslackConfiguration(IConfigurationBuilder configurationBuilder)
        {
            _messages = new Lazy<IReadOnlyDictionary<string, MessageConfiguration>>(LoadMessages);
            _appConfiguration = new Lazy<IEnumerable<ConfigurationGroup>>(LoadConfiguration);
            _defaultConfiguration = new Lazy<ConfigurationGroup>(configurationBuilder.CreateDefaultConfiguration);            
        }


        
        public ConfigurationGroup DefaultConfigurationGroup => _defaultConfiguration.Value;

        public IReadOnlyDictionary<string, MessageConfiguration> Messages => _messages.Value;

        public IEnumerable<ConfigurationGroup> Groups
        {
            get => _appConfiguration.Value;
            set
            {
                _appConfiguration = new Lazy<IEnumerable<ConfigurationGroup>>(() => value);
            }
        }
        

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="model"></param>
        public void SaveAppConfiguration(IEnumerable<ConfigurationGroup> model)
        {
            //update config in memory
            Groups = model ?? throw new ArgumentNullException(nameof(model));

            //update config in file
            var msgPath = IOHelper.MapPath(FilesLocation + "uslack.config");
            var json = JsonConvert.SerializeObject(model);

            File.WriteAllText(msgPath, json);
        }

        /// <summary>
        /// Gets a message body
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public MessageConfiguration GetMessage(string alias)
        {
            if (Messages.TryGetValue(alias.ToUpperInvariant(), out MessageConfiguration message))
            {
                return message;
            }

            throw new FileNotFoundException($"Content for alias {alias} couldn't be found");
        }

        private static IList<ConfigurationGroup> LoadConfiguration()
        {
            IList<ConfigurationGroup> config = null;
            var msgPath = IOHelper.MapPath(FilesLocation + "uslack.config");

            if (File.Exists(msgPath))
            {
                var content = File.ReadAllText(msgPath);
                config = JsonConvert.DeserializeObject<ConfigurationGroup[]>(content);
            }

            return config;
        }

        private static IReadOnlyDictionary<string, MessageConfiguration> LoadMessages()
        {
            var messages = new Dictionary<string, MessageConfiguration>();

            var msgPath = IOHelper.MapPath(FilesLocation);
            if (Directory.Exists(msgPath) == false) return null;
            var files = Directory.GetFiles(msgPath, "*.json");

            foreach (var file in files)
            {
                if (messages.ContainsKey(file)) continue;
                var content = File.ReadAllText(file);
                var config = JsonConvert.DeserializeObject<MessageConfiguration>(content);
                messages.Add(Path.GetFileNameWithoutExtension(file).ToUpperInvariant(), config);
            }

            return messages;
        }
    }
}
