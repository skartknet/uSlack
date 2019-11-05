using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace uSlack.Interactive
{
    /// <summary>
    /// We use this controller instead UmbracoApiController becuase we don't want them to be auto-routed.
    /// </summary>
    public abstract class InteractiveApiControllerBase : UmbracoApiControllerBase
    {
        public InteractiveApiControllerBase()
        {
        }


        protected InteractiveApiControllerBase(IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor, ISqlContext sqlContext, ServiceContext services, AppCaches appCaches, IProfilingLogger logger, IRuntimeState runtimeState, UmbracoHelper umbracoHelper)
            : base(globalSettings, umbracoContextAccessor, sqlContext, services, appCaches, logger, runtimeState, umbracoHelper)
        {

        }
    }
}
