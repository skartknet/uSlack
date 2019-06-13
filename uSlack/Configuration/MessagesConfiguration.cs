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
        internal Dictionary<string, string> Messages = new Dictionary<string, string>();

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
