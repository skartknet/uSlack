using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;
using uSlack.Models;

namespace uSlack.Controllers
{
    public class SlackInteractiveApiController : UmbracoApiController
    {
        [HttpPost]
        public void  Test(InteractiveResponse data)
        {
            var d = "test";
        }
    }
}
