using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
      [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class EventHandlerAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

            /// <summary>
            /// It registers a method as an event handler.
            /// </summary>
            /// <param name="alias">Unique alias for the handler</param>
            /// <param name="label">A label to be displayed in the backoffice</param>
            /// <param name="defaultValue">The default value for this handler</param>
        public EventHandlerAttribute(string alias, string label, bool defaultValue)
        {
            Alias = alias;
            DefaultValue = defaultValue;
            Label = label;
        }

        public string Alias { get; }
        public bool DefaultValue { get; }
        public string Label { get; }
    }

}
