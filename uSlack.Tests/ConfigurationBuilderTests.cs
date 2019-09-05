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
        public void ShouldReturnDefaultConfigurationGroup()
        {
            var sut = new ConfigurationBuilder();

            var config = sut.CreateDefaultConfiguration();

            Assert.AreEqual(config.Sections.Count, 4);
            Assert.IsNotNull(config.Sections["contentService"]);
            Assert.IsNotNull(config.Sections["mediaService"]);
            Assert.IsNotNull(config.Sections["memberService"]);
            Assert.IsNotNull(config.Sections["userService"]);

            Assert.AreEqual(config.Sections["contentService"].SectionHandlers.Count, 7);
            Assert.AreEqual(config.Sections["mediaService"].SectionHandlers.Count, 4);
            Assert.AreEqual(config.Sections["memberService"].SectionHandlers.Count, 2);
            Assert.AreEqual(config.Sections["userService"].SectionHandlers.Count, 2);

        }
    }



}
