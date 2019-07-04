// <copyright file="ConfigurationService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Umbraco.Core.IO;
using Umbraco.Web.Composing;
using uSlack.Configuration;

namespace uSlack
{
    public class UslackConfiguration : IConfiguration
    {
        const string _filesLocation = "~/App_Plugins/uSlack/Config/";
        private Lazy<IDictionary<string, MessageConfiguration>> _messages;
        private Lazy<IEnumerable<AppConfig>> _appConfiguration;
        private Lazy<AppConfig> _defaultConfiguration;

        private static UslackConfiguration _config;

        private UslackConfiguration()
        {
            _messages = new Lazy<IDictionary<string, MessageConfiguration>>(() =>
            {
                return InitializeMessages();
            });

            _appConfiguration = new Lazy<IEnumerable<AppConfig>>(() =>
            {
                return InitializeConfiguration();
            });

            _defaultConfiguration = new Lazy<AppConfig>(() =>
            {
                var builder = new ConfigurationBuilder();
                return builder.CreateDefaultConfiguration();
            });
        }


        public static UslackConfiguration Current
        {
            get
            {
                if (_config == null)
                {
                    _config = new UslackConfiguration();
                }

                return _config;
            }
        }

        public AppConfig DefaultConfiguration
        {
            get
            {
                return _defaultConfiguration.Value;
            }
        }
        public IDictionary<string, MessageConfiguration> Messages
        {
            get
            {
                return _messages.Value;
            }
        }

        public IEnumerable<AppConfig> AppConfiguration
        {
            get
            {
                return _appConfiguration.Value;
            }
            set
            {
                _appConfiguration = new Lazy<IEnumerable<AppConfig>>(() =>
                {
                    return value;
                });
            }
        }

        private static IList<AppConfig> InitializeConfiguration()
        {
            IList<AppConfig> config = null;
            var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");

            if (File.Exists(msgPath))
            {
                var content = File.ReadAllText(msgPath);
                config = JsonConvert.DeserializeObject<AppConfig[]>(content);
            }

            return config;
        }

        public T GetParameter<T>(int configIdx, string section, string parameter)
        {
            // warn: casting to int will give an error. Always cast to Int64;
            try
            {
                var list = AppConfiguration.ToList();
                var val = (T)list[configIdx].Sections[section].Parameters[parameter];
                return (T)val;
            }
            catch (KeyNotFoundException ex)
            {
                return default(T);
            }
        }

        private static Dictionary<string, MessageConfiguration> InitializeMessages()
        {
            var messages = new Dictionary<string, MessageConfiguration>();

            var msgPath = IOHelper.MapPath(_filesLocation);
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

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="model"></param>
        public void SaveAppConfiguration(IEnumerable<AppConfig> model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            //update config in memory
            AppConfiguration = model;

            //update config in file
            var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");
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

            throw new FileNotFoundException($"Content for alias {alias} couldn't found");

        }
    }
}
