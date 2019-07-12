using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace uSlack.Controllers.API
{
    public class ContentController : InteractiveController
    {
        
        public void Unpublish(string id)
        {
            var k = id;
        }
    }
}
