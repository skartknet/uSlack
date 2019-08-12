using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Umbraco.Core;
using Umbraco.Web.Security;

namespace uSlack.Security
{
    public class SecurityService : ISecurityService
    {
        private string HashMessage(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha1 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            return ToHex(hashmessage, false);

        }
        private string ToHex(byte[] bytes, bool upperCase = true)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        private bool IsValidSlackSignature(int timestamp, string payload, string slackSignature, string signingSecret)
        {
            if (string.IsNullOrWhiteSpace(signingSecret))
                throw new ArgumentNullException(nameof(signingSecret));

            string versionNo = "v0";
            var basestring = $"{versionNo}:{timestamp}:{payload}";

            var hashvalue = HashMessage(signingSecret, basestring);

            var computedSignature = "v0=" + hashvalue;

            return computedSignature.Equals(slackSignature);
        }

        public async Task<bool> IsValidRequestAttemptAsync(HttpRequestMessage request)
        {
            var rawPayload = await request.Content.ReadAsStringAsync();
            var slackSignature = request.Headers.GetValues("X-Slack-Signature").FirstOrDefault();
            var timestampString = request.Headers.GetValues("X-Slack-Request-Timestamp").FirstOrDefault();
            var signingSecret = ConfigurationManager.AppSettings["SlackSigningSecret"];

            if (slackSignature.IsNullOrWhiteSpace() || timestampString.IsNullOrWhiteSpace()) return false;
            var timestamp = int.Parse(timestampString);

            if (DateTimeOffset.Now.ToUnixTimeSeconds() - timestamp > 60 * 5) return false;
            if (signingSecret.IsNullOrWhiteSpace()) return false;

            return IsValidSlackSignature(timestamp, rawPayload, slackSignature, signingSecret);
        }
    }

    public interface ISecurityService
    {
        Task<bool> IsValidRequestAttemptAsync(HttpRequestMessage request);
    }
}
