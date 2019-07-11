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

namespace uSlack.Controllers
{
    //[AuthorizeSlack]
    public class InteractiveApiController : UmbracoApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> ProcessResponse()
        {
            //var rawPayload = response.Get("payload");
            var rawPayload = await Request.Content.ReadAsStringAsync();

            var content = await Request.Content.ReadAsFormDataAsync();

            var responseModel = JsonConvert.DeserializeObject<InteractiveResponse>(content.Get("payload"));

            //string rawPayload = await Read(this.Request);


            var slackSignature = Request.Headers.GetValues("X-Slack-Signature").FirstOrDefault();
            var timestampString = Request.Headers.GetValues("X-Slack-Request-Timestamp").FirstOrDefault();

            if (slackSignature.IsNullOrWhiteSpace() || timestampString.IsNullOrWhiteSpace()) return Unauthorized();

            if (!int.TryParse(timestampString, out int timestamp)) return BadRequest();

            if (DateTimeOffset.Now.ToUnixTimeSeconds() - timestamp > 60 * 5) return BadRequest();

            var signingSecret = ConfigurationManager.AppSettings["SlackSigningSecret"];

            var isValid = uSlack.Security.Security.IsValidSlackSignature(timestamp, rawPayload, slackSignature, signingSecret);

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
