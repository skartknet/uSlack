using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;
using uSlack.Services;

namespace uSlack.Tests
{
    [TestFixture]
    public class Extensions
    {
        [Test]
        public void ShouldReplaceOnePlaceholder()
        {           
            var txt = "some text here {name} and more text";
            var properties = new Dictionary<string, string>();
            properties.Add("name", "node1Name");

            var txtReplaced = txt.ReplacePlaceholders(properties);


            Assert.That(txtReplaced, Is.EqualTo("some text here node1Name and more text"));
        }

        [Test]
        public void ShouldReplaceTwoPlaceholders()
        {           
            var properties = new Dictionary<string, string>();
            properties.Add("name", "node1Name");
            properties.Add("id", "1234");


            var txt = "some text here {name} and more text and {name} and other {id}";
            var txtReplaced = txt.ReplacePlaceholders(properties);

            Assert.That(txtReplaced, Is.EqualTo("some text here node1Name and more text and node1Name and other 1234"));
        }
    }
}
