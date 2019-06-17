// <copyright file="ConfigurationService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Umbraco.Core.IO;
using Umbraco.Web.Composing;
using uSlack.Configuration;

namespace uSlack
{
    public class UslackConfiguration : IConfiguration
    {
        const string _filesLocation = "~/App_Plugins/uSlack/Config/";
        private Lazy<Dictionary<string, string>> _messages;
        private Lazy<AppConfiguration> _appConfiguration;

        private static UslackConfiguration _config;

        private UslackConfiguration()
        {
            _messages = new Lazy<Dictionary<string, string>>(() =>
            {
                return InitializeMessages();
            });

            _appConfiguration = new Lazy<AppConfiguration>(() =>
            {
                return InitializeConfiguration();
            });
        }


        public static UslackConfiguration Current
        {
            get
            {
                if(_config == null)
                {
                    _config = new UslackConfiguration();
                }

                return _config;
            }
        }


        public Dictionary<string, string> Messages
        {
            get
            {
                return _messages.Value;
            }
        }

        public AppConfiguration AppConfiguration
        {
            get
            {
                return _appConfiguration.Value;
            }
            set
            {
                _appConfiguration = new Lazy<AppConfiguration>(() =>
                {
                    return value;
                });
            }
        }

        private static AppConfiguration InitializeConfiguration()
        {
            AppConfiguration config = null;
            var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");

            if (File.Exists(msgPath))
            {
                var content = File.ReadAllText(msgPath);
                config = JsonConvert.DeserializeObject<AppConfiguration>(content);
            }

            return config;
        }

        private static Dictionary<string, string> InitializeMessages()
        {
            var messages = new Dictionary<string, string>();

            var msgPath = IOHelper.MapPath(_filesLocation);
            if (Directory.Exists(msgPath) == false) return null;
            var files = Directory.GetFiles(msgPath, "*.json");

            foreach (var file in files)
            {
                if (messages.ContainsKey(file)) continue;
                var content = File.ReadAllText(file);
                messages.Add(Path.GetFileNameWithoutExtension(file).ToUpperInvariant(), content);
            }

            return messages;
        }

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="model"></param>
        public void SaveAppConfiguration(AppConfiguration model)
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
