﻿using Umbraco.Core.Composing;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class TypesRegistration : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(typeof(ContentHandlers));

            composition.Register(typeof(IMessageService), typeof(SlackService));
            composition.Register(typeof(IAppConfiguration), typeof(AppConfiguration), Lifetime.Singleton);
        }
    }
}
