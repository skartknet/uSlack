using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace uSlack.Backoffice
{
    public class SlackErrorResult : IHttpActionResult
    {
        readonly string _error;
        readonly HttpRequestMessage _request;

        public SlackErrorResult(string error, HttpRequestMessage request)
        {
            _error = error;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            string message;
            switch (_error)
            {
                case "missing_scope":
                    message = "The token used is not granted the specific scope permissions required to complete this request.";
                    break;
                case "not_authed":
                    message = "No authentication token provided.";
                    break;
                case "invalid_auth":
                    message = "Some aspect of authentication cannot be validated. Either the provided token is invalid or the request originates from an IP address disallowed from making the request.";
                    break;
                case "account_inactive":
                    message = "Authentication token is for a deleted user or workspace.";
                    break;
                case "token_revoked":
                    message = "Authentication token is for a deleted user or workspace or the app has been removed.";
                    break;
                case "no_permission":
                    message = "The workspace token used in this request does not have the permissions necessary to complete the request. ";
                    break;
                case "ekm_access_denied":
                    message = "Administrators have suspended the ability to post a message.";
                    break;
                default:
                    message = "Undefined error";
                    break;
            }
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                RequestMessage = _request,
                Content = new StringContent(message)
            };

            return Task.FromResult(response);
        }
    }
}
