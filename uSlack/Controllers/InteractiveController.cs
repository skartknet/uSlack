using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.WebApi;
using System.Web.Http;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SlackAPI;
using Umbraco.Core;
using uSlack.Models;

namespace uSlack.Controllers
{
    //[AuthorizeSlack]
    public abstract class InteractiveApiController : UmbracoApiController
    {
        public IHttpActionResult ProcessResponse([FromBody]string response)
        {
            var obj = JsonConvert.DeserializeObject<InteractiveResponse>(response);

            var signingSecret = ConfigurationManager.AppSettings["SlackSigningSecret"];
            if (string.IsNullOrWhiteSpace(signingSecret))
                return Unauthorized();

            var slackSignature = Request.Headers.GetValues("X-Slack-Signature").FirstOrDefault();
            var timestampString = Request.Headers.GetValues("X-Slack-Request-Timestamp").FirstOrDefault();

            if (slackSignature.IsNullOrWhiteSpace() || timestampString.IsNullOrWhiteSpace()) return Unauthorized();

            if (!int.TryParse(timestampString, out int timestamp)) return BadRequest();

            if (DateTimeOffset.Now.ToUnixTimeSeconds() - timestamp > 60 * 5) return BadRequest();

            return Ok();
        }
    }
}
