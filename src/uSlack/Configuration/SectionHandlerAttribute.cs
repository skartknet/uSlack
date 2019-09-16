﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Configuration
{
    /// <summary>
    /// It registers a class as a configuration section.
    /// </summary>

    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SectionHandlerAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        public SectionHandlerAttribute(string alias, string label)
        {
            this.Alias = alias;
            this.Label = label;
        }

        public string Alias { get; }
        public string Label { get; }

    }

}
