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
    public class AppConfigurationTests
    {
        private ConfigurationGroup _config;

        [SetUp]
        public void CreateConfiguration()
        {
            _config = new ConfigurationGroup();

            var sections = new Dictionary<string, ConfigSection>();
            sections.Add("contentService", new ConfigSection
            {
                Parameters = new Dictionary<string, object>
                {
                    {"create", true },
                    {"delete", false }
                }
            });

            _config.Sections = sections;

        }

        [Test]
        public void ShouldReturnParameter()
        {
            var paramTrue = _config.GetParameter<bool>("contentService", "create");
            var paramFalse = _config.GetParameter<bool>("contentService", "delete");

            Assert.That(paramTrue, Is.True);
            Assert.That(paramFalse, Is.False);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            var paramFalse = _config.GetParameter<bool>("unknownService", "unknownMethod");

            Assert.That(paramFalse, Is.False);
        }
    }
}
