using System;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class MemberHandlers : EventHandlerBase
    {

        public MemberHandlers(IMessageService messageService,
                              IAppConfiguration config) : base(messageService, config)
        { }

        public void MemberService_Deleted(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMember> e)
        {
            throw new NotImplementedException();
        }

        public void MemberService_Saved(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMember> e)
        {
            throw new NotImplementedException();
        }

    }
}
