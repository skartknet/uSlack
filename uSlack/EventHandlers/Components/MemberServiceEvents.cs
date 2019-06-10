// https://our.umbraco.com/Documentation/implementation/composing/

using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace uSlack.EventHandlers.Components
{
    public class MemberServiceEvents : IComponent
    {
        private readonly MemberHandlers _handlers;

        public MemberServiceEvents(MemberHandlers handlers)
        {
            _handlers = handlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            MemberService.Saved += _handlers.MemberService_Saved;
            MemberService.Deleted += _handlers.MemberService_Deleted;
        }


        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class USlackMemberServiceComposer : ComponentComposer<MemberServiceEvents>
    {
        // nothing needed to be done here!
    }
}