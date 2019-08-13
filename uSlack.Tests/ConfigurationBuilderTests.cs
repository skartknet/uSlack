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
            var sut = new TestConfigBuilder();

            var testingObj = new TestConfigBuilder();
            testingObj.TestBuildSections();
        }
    }

    [SectionHandler("contentService")]
    public class TestSection
    {
        [EventHandler("published", true)]
        public void TestEventHandler() { }
    }

    //To test protected methods
    //public class TestConfigBuilder : ConfigurationBuilder
    //{
    //    public void TestBuildSections()
    //    {
    //        var types = new Type[] { typeof(TestSection) };

    //        var result = this.BuildSections(types);

    //        Assert.That(result.ContainsKey("contentService"), Is.True);
    //        Assert.That(result["contentService"].Parameters.ContainsKey("published"), Is.True);
    //    }
    //}
}
