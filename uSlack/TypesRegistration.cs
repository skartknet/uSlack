using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class TypesRegistration : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(typeof(IContentHandlers), typeof(ContentHandlers));
            composition.Register(typeof(IMessageService), typeof(SlackService));
            composition.Register(typeof(AppConfiguration), Lifetime.Singleton);
        }
    }
}
