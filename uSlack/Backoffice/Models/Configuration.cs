using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.Backoffice.Models
{
    public class Configuration
    {
        public string Token { get; set; }

        public string SlackChannel { get; private set; }

        public List<ConfigSection> Sections { get; set; }
    }
}
