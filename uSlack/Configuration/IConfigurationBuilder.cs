using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfigurationBuilder
    {
        Dictionary<string, ConfigSection> BuildSections(IEnumerable<System.Type> registeredSections);
        AppConfiguration CreateDefaultConfiguration();
    }
}