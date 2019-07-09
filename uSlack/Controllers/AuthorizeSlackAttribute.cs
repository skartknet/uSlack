using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using uSlack.Models;

namespace uSlack.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeSlackAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentException(nameof(actionContext));
            if (this.IsAuthorized(actionContext))
                return;
            this.HandleUnauthorizedRequest(actionContext);
        }

        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentException(nameof(actionContext));

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Signing Secret couldn't be authorized.");
        }

        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentException(nameof(actionContext));

            //TODO: move the setting to its own model and check the null there.
            var signingSecret = ConfigurationManager.AppSettings["SlackSigningSecret"];
            if (string.IsNullOrWhiteSpace(signingSecret))
                return false;

            var slackSignature = actionContext.Request.Headers.GetValues("X-Slack-Signature").FirstOrDefault();
            if (slackSignature == null) return false;

            var body = Task.Run(async () => await actionContext.Request.Content.ReadAsAsync<InteractiveResponse>()).Result;

            return true;
        }
    }
}