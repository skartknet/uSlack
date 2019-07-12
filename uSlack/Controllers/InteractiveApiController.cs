using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Umbraco.Web.WebApi;
using System.Web.Http;
using Newtonsoft.Json;
using Umbraco.Core;
using uSlack.Models;
using uSlack.Services;

namespace uSlack.Controllers
{
    
    public class InteractiveApiController : UmbracoApiController
    {
        private readonly IInteractiveRouter _interactiveRouter;

        public InteractiveApiController(IInteractiveRouter interactiveRouter)
        {
            _interactiveRouter = interactiveRouter;
        }

        [HttpPost]
        public async Task<IHttpActionResult> ProcessResponse()
        {


            var rawPayload = await Request.Content.ReadAsStringAsync();
            var slackSignature = Request.Headers.GetValues("X-Slack-Signature").FirstOrDefault();
            var timestampString = Request.Headers.GetValues("X-Slack-Request-Timestamp").FirstOrDefault();
            var signingSecret = ConfigurationManager.AppSettings["SlackSigningSecret"];

            if (slackSignature.IsNullOrWhiteSpace() || timestampString.IsNullOrWhiteSpace()) return Unauthorized();
            if (!int.TryParse(timestampString, out int timestamp)) return BadRequest();
            if (DateTimeOffset.Now.ToUnixTimeSeconds() - timestamp > 60 * 5) return BadRequest();
            if (signingSecret.IsNullOrWhiteSpace()) return InternalServerError();
            

            var isValidSignature = uSlack.Security.Security.IsValidSlackSignature(timestamp, rawPayload, slackSignature, signingSecret);

            if (!isValidSignature) return Unauthorized();

            var content = await Request.Content.ReadAsFormDataAsync();
            var responseModel = JsonConvert.DeserializeObject<InteractiveResponse>(content.Get("payload"));
            foreach (var action in responseModel.Actions)
            {
                _interactiveRouter.RouteRequestValue();
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
