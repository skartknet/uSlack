using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Umbraco.Core.IO;

namespace uSlack.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        const string _filesLocation = "~/App_Plugins/uSlack/Config/";

        public MessagesConfiguration MessagesConfiguration { get; }
        public UslackConfiguration AppConfiguration { get; }

        public ConfigurationService()
        {
            MessagesConfiguration = new MessagesConfiguration();
            AppConfiguration = new UslackConfiguration();
        }

        public void Initialize()
        {
            InitializeConfiguration();
            InitializeMessages();
        }

        private void InitializeConfiguration()
        {
            try
            {
                var msgPath = IOHelper.MapPath(_filesLocation + "uslack.config");
                var content = File.ReadAllText(msgPath);

                JsonConvert.PopulateObject(content, AppConfiguration);
            }
            catch (Exception ex)
            {
                //TODO: add logging
                throw ex;
            }
        }

        private void InitializeMessages()
        {
            var msgPath = IOHelper.MapPath(_filesLocation);
            if (Directory.Exists(msgPath) == false) return;
            var files = Directory.GetFiles(msgPath, "*.json");

            foreach (var file in files)
            {
                if (MessagesConfiguration.Messages.ContainsKey(file)) continue;
                var content = File.ReadAllText(file);
                MessagesConfiguration.Messages.Add(Path.GetFileNameWithoutExtension(file).ToUpperInvariant(), content);
            }
        }

    }
}
