using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Umbraco.Core.IO;

namespace uSlack.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly string _configFileLocation = "~/App_Plugins/uSlack/Config/uslack.config";

        [JsonIgnore]
        public MessagesConfiguration Messages { get; }

        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("channel")]
        public string SlackChannel { get; private set; }

        [JsonProperty("sections")]
        public List<ConfigSection> Sections { get; set; }

        public AppConfiguration()
        {
            Messages = new MessagesConfiguration();
            Messages.Initialize();
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                var msgPath = IOHelper.MapPath(_configFileLocation);
                var content = File.ReadAllText(msgPath);

                JsonConvert.PopulateObject(content, this);
            }
            catch (Exception ex)
            {
                //TODO: add logging
                throw ex;
            }
        }


    }
}
