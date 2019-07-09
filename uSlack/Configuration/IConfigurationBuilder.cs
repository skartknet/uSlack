using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfigurationBuilder
    {
        ConfigurationGroup CreateDefaultConfiguration();
    }
}