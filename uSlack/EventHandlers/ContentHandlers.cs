using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.EventHandlers
{
    public class ContentHandlers : IContentHandlers
    {
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
