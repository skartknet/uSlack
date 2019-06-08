using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.IO;

namespace uSlack.Configuration
{
    public class MessagesConfiguration
    {
        private string _filesLocation = "~/App_Plugins/uSlack/Config";

        private Dictionary<string, string> _messages = new Dictionary<string, string>();


        public void Initialize()
        {
            var msgPath = IOHelper.MapPath(_filesLocation);
            if (Directory.Exists(msgPath) == false) return;
            var files = Directory.GetFiles(msgPath, "*.json");

            foreach (var file in files)
            {
                if (_messages.ContainsKey(file)) continue;
                var content = File.ReadAllText(file);
                _messages.Add(Path.GetFileNameWithoutExtension(file).ToUpperInvariant(), content);
            }
        }

        public string GetMessage(string alias)
        {
            if (_messages.TryGetValue(alias.ToUpperInvariant(), out string message))
            {
                return message;
            }

            throw new FileNotFoundException($"Content for alias {alias} couldn't found");

        }
    }
}
