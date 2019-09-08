using Newtonsoft.Json;
using SlackAPI.Interactive;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using uSlack.Security;
using uSlack.Services;
using uSlack.Services.Models;

namespace uSlack.Interactive
{
    [PluginController("uslack")]
    public class InteractiveApiController : UmbracoApiController
    {
        private readonly InteractiveControllerSelector _controllerSelector;
        private readonly ISecurityService _securityService;
        private readonly ILogger _logger;

        public InteractiveApiController(InteractiveControllerSelector controllerSelector,
                                        ISecurityService securityService,
                                        ILogger logger)
        {
            _controllerSelector = controllerSelector;
            _securityService = securityService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IHttpActionResult> ProcessResponse()
        {

            var isValidSignature = await _securityService.IsValidRequestAttemptAsync(Request);

            if (!isValidSignature) return Unauthorized();

            var content = await Request.Content.ReadAsFormDataAsync();
            try
            {
                var responseModel = JsonConvert.DeserializeObject<InteractiveResponse>(content.Get("payload"));

                foreach (var action in responseModel.Actions)
                {
                    var route = new InteractiveRoute
                    {
                        Controller = action.BlockId,
                        Method = action.ActionId
                    };

                    route.Value = action.Value;

                    var controller = _controllerSelector.SelectController(route.Controller);


                    var methodInfo = controller.ControllerType.GetMethod(route.Method, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (methodInfo == null) throw new Exception("Method not found");

                    object classInstance = Activator.CreateInstance(controller.ControllerType, null);
                    if (route.Value.IsNullOrWhiteSpace())
                    {
                        return (IHttpActionResult)methodInfo.Invoke(classInstance, null);
                    }
                    else
                    {
                        return (IHttpActionResult)methodInfo.Invoke(classInstance, new object[] { route.Value });
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error<InteractiveApiController>(ex, "Error processing interactive request");
                return InternalServerError();
            }

            return Ok();
        }

    }
}
