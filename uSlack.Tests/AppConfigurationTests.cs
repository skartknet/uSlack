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
        private AppConfiguration _appConfig;

        [SetUp]
        public void CreateConfiguration()
        {
            _appConfig = new AppConfiguration();

            var sections = new Dictionary<string, ConfigSection>();
            sections.Add("contentService", new ConfigSection
            {
                Parameters = new Dictionary<string, object>
                {
                    {"create", true },
                    {"delete", false }
                }
            });

            _appConfig.Sections = sections;

        }

        [Test]
        public void ShouldReturnParameter()
        {
            var paramTrue = _appConfig.GetParameter<bool>("contentService", "create");
            var paramFalse = _appConfig.GetParameter<bool>("contentService", "delete");

            Assert.That(paramTrue, Is.True);
            Assert.That(paramFalse, Is.False);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            var paramFalse = _appConfig.GetParameter<bool>("unknownService", "unknownMethod");

            Assert.That(paramFalse, Is.False);
        }
    }
}
