using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Services.Models
{
    public class InteractiveControllerDescriptor
    {
        public string ControllerName { get; set; }
        public Type ControllerType { get; set; }
        public InteractiveControllerDescriptor(string controllerName, Type controllerType)
        {
            ControllerName = controllerName;
            ControllerType = controllerType;
        }
    }
}
