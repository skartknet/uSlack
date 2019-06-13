using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.IO;
using uSlack.Configuration;

namespace uSlack.Tests
{
    [TestFixture]
    public class MessagesConfigurationShould
    {

        [Test]
        public void ReturnsFileContent()
        {
            var sut = new ConfigurationService();
            sut.Initialize();

            var fileContent = sut.MessagesConfiguration.GetMessage("filetest");
            Assert.That(fileContent, Is.EqualTo("testcontent"));
        }

        [Test]
        public void ReturnsNotFoundExceptionForWrongAlias()
        {
            var sut = new ConfigurationService();
            sut.Initialize();

            Assert.Throws<FileNotFoundException>(() => sut.MessagesConfiguration.GetMessage("wrongAlias"));
        }
    }
}
