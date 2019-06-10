using System;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class UserHandlers : EventHandlerBase
    {

        public UserHandlers(IMessageService messageService,
                              IAppConfiguration config) : base(messageService, config)
        { }

        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            throw new NotImplementedException();
        }

        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            throw new NotImplementedException();
        }

        public void UserService_SavedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            //throw new NotImplementedException();
        }

    }
}
