using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    /// <summary>
    /// It registers a method or the methods of a class as a property of the uSlack configuration.
    /// </summary>

    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class EventHandlerAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        public EventHandlerAttribute(string alias, object defaultValue)
        {
            Alias = alias;
            DefaultValue = defaultValue;
        }

        public string Alias { get; }
        public object DefaultValue { get; }
    }

}
