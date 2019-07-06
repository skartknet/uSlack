using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Search;
using uSlack.Configuration;

namespace uSlack.Configuration
{
    public class DefaultConfiguration : ConfigurationBuilder
    {

        public override AppConfiguration Configure(AppConfiguration config)
        {
            config.AddSection("alias", config=> {
                config.SetParameter("")

            })
        }

    }
}
