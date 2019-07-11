using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Security
{
    public class Security
    {
        private static string HashMessage(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha1 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            return ToHex(hashmessage, false);

        }
        private static string ToHex(byte[] bytes, bool upperCase = true)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static bool IsValidSlackSignature(int timestamp, string payload, string slackSignature, string signingSecret)
        {


            if (string.IsNullOrWhiteSpace(signingSecret))
                throw new ArgumentNullException(nameof(signingSecret));

            string versionNo = "v0";
            var basestring = $"{versionNo}:{timestamp}:{payload}";

            var hashvalue = HashMessage(signingSecret, basestring);

            var computedSignature = "v0=" + hashvalue;

            return computedSignature.Equals(slackSignature);
        }
    }
}
