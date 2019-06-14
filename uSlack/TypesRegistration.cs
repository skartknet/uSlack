﻿// <copyright file="TypesRegistration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using SlackAPI;
using Umbraco.Core.Composing;
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


            composition.Register(typeof(SlackClientBase), typeof(SlackTaskClient));

            composition.Register(typeof(IMessageService), typeof(SlackService));
    
            composition.Register(typeof(IConfigurationService), typeof(ConfigurationService), Lifetime.Singleton);
        }
    }
}
