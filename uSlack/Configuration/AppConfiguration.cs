using System.Configuration;

namespace uSlack.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public MessagesConfiguration Messages { get; }

        public string Token => ConfigurationManager.AppSettings["SlackAppToken"];

        public string SlackChannel => ConfigurationManager.AppSettings["SlackAppChannel"];

        public AppConfiguration()
        {
            Messages = new MessagesConfiguration();
            Messages.Initialize();
        }


    }
}
