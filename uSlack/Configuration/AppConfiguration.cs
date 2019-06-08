using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public class AppConfiguration
    {
        public MessagesConfiguration Messages { get; }

        public string Token => ConfigurationManager.AppSettings["SlackAppToken"];

        public AppConfiguration()
        {
            Messages = new MessagesConfiguration();
        }

        
    }
}
