using System;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;
using uSlack.Configuration;

namespace uSlack.EventHandlers
{
    public class UserHandlers : EventHandlerBase
    {

        public UserHandlers(IMessageService messageService,
                              IConfigurationService config) : base(messageService, config)
        { }

        public void UserService_DeletedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "User has been deleted", nameof(this.UserService_DeletedUser)));
            }
        }

        public void UserService_DeletedUserGroup(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.Membership.IUserGroup> e)
        {
            foreach (IUserGroup item in e.DeletedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "User group has been deleted", nameof(this.UserService_DeletedUserGroup)));
            }
        }

        public void UserService_SavedUser(Umbraco.Core.Services.IUserService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.Membership.IUser> e)
        {
            foreach (var item in e.SavedEntities)
            {
                Task.Run(async () => await SendMessageAsync(item, "User has been saved", nameof(this.UserService_SavedUser)));
            }
        }

    }
}
