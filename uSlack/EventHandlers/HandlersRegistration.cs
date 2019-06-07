using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;

namespace uSlack.EventHandlers
{
    public class HandlersRegistration : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(typeof(IContentHandlers), typeof(ContentHandlers));
        }
    }
}
