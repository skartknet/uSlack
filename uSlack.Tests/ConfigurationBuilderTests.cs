using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.Tests
{
    [TestFixture]
    public class ConfigurationBuilderTests
    {
        [Test]
        public void ShouldReturnSectionsDictionary()
        {
            var types = new Type[] { typeof(TestSection) };
            var sut = new ConfigurationBuilder();

            var result = sut.BuildSections(types);

            Assert.That(result.ContainsKey("contentService"), Is.True);
            Assert.That(result["contentService"].Parameters.ContainsKey("published"), Is.True);

        }
    }

    [SectionHandler("contentService")]
    public class TestSection
    {
        [EventHandler("published", true)]
        public void TestEventHandler() { }
    }
}
