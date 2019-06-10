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
        public void ShouldReplaceTwoPlaceholders()
        {
            var node = new Mock<IContent>();
            node.SetupGet(n => n.Name).Returns("My Content Name");
            node.SetupGet(n => n.Id).Returns(1234);


            var txt = "some text here @{Name} and more text and @{Name} and other @{Id}";
            var txtReplaced = txt.ReplacePlaceholders(node.Object);


            Assert.That(txtReplaced, Is.EqualTo("some text here My Content Name and more text and My Content Name and other 1234"));
        }
    }
}
