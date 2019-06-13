using System;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Core.Models.Entities;

namespace uSlack.Extensions
{
    public static class StringExtensions
    {
        public static string ReplacePlaceholders(this string txt, IEntity node)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                throw new ArgumentException("The text is empty", nameof(txt));
            }

            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            string resultTxt = txt;
            foreach (Match ph in Regex.Matches(txt, "@{(\\w+)}"))
            {
                var placeholder = ph.Value;
                var placehPropName = ph.Groups[1].Value;

                var matchedPropInfo = node.GetType().GetPublicProperties()
                    .FirstOrDefault(prp => prp.Name.Equals(placehPropName));

                if (matchedPropInfo == null) continue;

                try
                {
                    var matchedValue = matchedPropInfo.GetValue(node).ToString();
                    resultTxt = resultTxt.Replace(placeholder, matchedValue);
                }
                catch (Exception ex) {
                    throw ex;
                    //TODO: log error
                    /*we ignore if any errors and continue with next property*/}
            }

            return resultTxt;

        }
    }
}
