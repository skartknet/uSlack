using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class EventHandlerAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string alias;

        // This is a positional argument
        public EventHandlerAttribute(string alias)
        {
            this.alias = alias;
        }

        public string Alias
        {
            get { return alias; }
        }

    }
}
