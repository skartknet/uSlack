using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace uSlack.Tests
{
    [TestFixture]
    public class Extensions
    {
        [Test]
        public void ShouldReplaceOnePlaceholder()
        {
            var node = new Mock<IContent>();
            node.SetupGet(n => n.Name).Returns("My Content Name");

            var txt = "some text here @{Name} and more text";
            var txtReplaced = txt.ReplacePlaceholders(node.Object);


            Assert.That(txtReplaced, Is.EqualTo("some text here My Content Name and more text"));
        }

        [Test]
        [Ignore("TODO: accept placeholders for custom properties")]
        public void ShouldReplaceTwoPlaceholders()
        {
            var node = new Mock<IContent>();
            node.SetupGet(n => n.Name).Returns("My Content Name");

            var txt = "some text here @{Name} and more text and @{Name} and other @{}";
            var txtReplaced = txt.ReplacePlaceholders(node.Object);


            Assert.That(txtReplaced, Is.EqualTo("some text here My Content Name and more text and My Content Name and other Property"));
        }
    }
}
