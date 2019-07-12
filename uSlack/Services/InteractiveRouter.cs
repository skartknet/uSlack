using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace uSlack.Services
{
    public class InteractiveRouter : DefaultHttpControllerSelector,  IInteractiveRouter
    {
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;

        public InteractiveRouter(HttpConfiguration configuration) : base(configuration)
        {
            this._controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(new Func<ConcurrentDictionary<string, HttpControllerDescriptor>>(this.InitializeControllerInfoCache));

        }

        public void RouteRequestValue(string value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/content/unpublish");
            var ctrl = SelectController(request);
        }

      
    }
}
