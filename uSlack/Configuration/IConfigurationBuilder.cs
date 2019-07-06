using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public interface IConfigurationBuilder
    {
        AppConfiguration CreateDefaultConfiguration();
        Dictionary<string, ConfigSection> BuildSections(IEnumerable<Type> registeredSections);
    }
}
