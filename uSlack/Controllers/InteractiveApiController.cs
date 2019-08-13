using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Umbraco.Web.WebApi;
using System.Web.Http;
using Newtonsoft.Json;
using uSlack.Security;
using uSlack.Services;
using uSlack.Services.Models;
using uSlack.Models;

namespace uSlack.Controllers
{

    public class InteractiveApiController : UmbracoApiController
    {
        private readonly InteractiveControllerSelector _controllerSelector;
        private readonly ISecurityService _securityService;

        public InteractiveApiController(InteractiveControllerSelector controllerSelector,
                                        ISecurityService securityService)
        {
            _controllerSelector = controllerSelector;
            _securityService = securityService;
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
                    Method = action.action_id
                };



                switch (action)
                {
                    case ButtonElementInteractive act:
                        route.Value = act.value;
                        break;
                    case DatePickerElementInteractive act:
                        route.Value = act.SelectedDate.ToString("yy-MM-dd");
                        break;
                }


                var controller = _controllerSelector.SelectController(route.Controller);


                var methodInfo = controller.ControllerType.GetMethod(route.Method);
                if (methodInfo == null) throw new Exception("Method not found");

                //methodInfo.Invoke()

            }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }


        private async Task<string> Read(HttpRequestMessage req)
        {
            using (var contentStream = await req.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

    }
}
