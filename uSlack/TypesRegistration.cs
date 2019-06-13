﻿using Umbraco.Core.Composing;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.EventHandlers
{
    public class TypesRegistration : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(typeof(ContentHandlers));
            composition.Register(typeof(MediaHandlers));
            composition.Register(typeof(UserHandlers));
            composition.Register(typeof(MemberHandlers));

            composition.Register(typeof(IMessageService), typeof(SlackService));
    
            composition.Register(typeof(IConfigurationService), typeof(ConfigurationService), Lifetime.Singleton);
        }
    }
}
