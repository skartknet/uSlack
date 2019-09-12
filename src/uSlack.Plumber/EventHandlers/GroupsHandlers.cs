using System;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.Plumber.EventHandlers
{
    [SectionHandler("plumberGroups", "Plumber Groups")]
    public class GroupsHandlers
    {
        private readonly IMessageService _messagingService;

        public GroupsHandlers(IMessageService messagingService)
        {
            _messagingService = messagingService;
        }

        //[EventHandler("created", true)]
        //public static void GroupService_Created(object sender, Workflow.Events.Args.GroupEventArgs e)
        //{
        //    //foreach (var item in e.Group)
        //    //{
        //    //    var properties = new PropertiesDictionary(item);
        //    //    var publisher = Current.Services.UserService.GetUserById(item.PublisherId.GetValueOrDefault());
        //    //    if (publisher != null) properties.Add("publisher", publisher.Name);

        //    //    AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "published", properties));
        //    //}
        //}
    }
}
