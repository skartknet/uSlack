// https://our.umbraco.com/Documentation/implementation/composing/

using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace uSlack.EventHandlers.Components
{
    public class UserServiceEvents : IComponent
    {
        private readonly UserHandlers _handlers;

        public UserServiceEvents(UserHandlers handlers)
        {
            _handlers = handlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            UserService.SavedUser += _handlers.UserService_SavedUser;
            UserService.DeletedUserGroup += _handlers.UserService_DeletedUserGroup;
            UserService.DeletedUser += _handlers.UserService_DeletedUser;
        }


        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class USlackUserServiceComposer : ComponentComposer<UserServiceEvents>
    {
        // nothing needed to be done here!
    }
}