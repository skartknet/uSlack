// <copyright file="InitUSlack.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

// https://our.umbraco.com/Documentation/implementation/composing/


using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;
using uSlack.EventHandlers;
using uSlack.Plumber.EventHandlers;
using Workflow.Services;

namespace uSlack
{
    public class InitPlumberHandlers : IComponent
    {
        private readonly ContentHandlers _contentHandlers;

        public InitPlumberHandlers(ContentHandlers contentHandlers)
        {
            _contentHandlers = contentHandlers;       
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {

            try
            {
                //GroupService.Created += GroupsHandlers.GroupService_Created;
            }
            catch { }
        }

    


        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            ContentService.Published -= _contentHandlers.ContentService_Published; 
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class InitUSlackComposer : ComponentComposer<InitUSlack>
    {
        // nothing needed to be done here!
    }
}