using System;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class MemberHandlers : EventHandlerBase
    {

        public MemberHandlers(IMessageService messageService,
                              IConfigurationService config) : base(messageService, config)
        { }

        public void MemberService_Deleted(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMember> e)
        {
            foreach (IMember item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Member has been deleted", nameof(this.MemberService_Deleted)));
            }
        }

        public void MemberService_Saved(Umbraco.Core.Services.IMemberService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMember> e)
        {
            foreach (IMember item in e.SavedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "Member has been saved", nameof(this.MemberService_Deleted)));
            }
        }

    }
}
