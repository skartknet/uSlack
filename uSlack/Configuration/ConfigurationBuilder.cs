using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    public abstract class ConfigurationBuilder
    {
        public abstract AppConfiguration Configure(AppConfiguration config);
    }
}
