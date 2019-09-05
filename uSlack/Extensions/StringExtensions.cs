// <copyright file="StringExtensions.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Core.Models.Entities;

namespace uSlack.Services
{
    public static class StringExtensions
    {
        public static string ReplacePlaceholders(this string txt, IDictionary<string, string> properties)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                throw new ArgumentException("The text is empty", nameof(txt));
            }

            string resultTxt = txt;
            foreach (Match ph in Regex.Matches(txt, "@{(\\w+)}"))
            {
                var placeholder = ph.Value;
                var placehPropName = ph.Groups[1].Value;

                string matchedValue = properties.ContainsKey(placehPropName) ? properties[placehPropName] : default;
                if(matchedValue != null)
                {
                     resultTxt = resultTxt.Replace(placeholder, matchedValue);
                }
            }

            return resultTxt;

        }
    }
}
