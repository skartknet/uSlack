using NUnit.Framework;
using System.IO;
using Umbraco.Core.IO;
using uSlack.Configuration;
using uSlack.EventHandlers.Components;

namespace uSlack.Tests
{
    [TestFixture]
    public class AppConfigurationShould
    {
        [SetUp]
        public void CreateConfigurationFile()
        {
            string configFileLocation = "~/App_Plugins/uSlack/Config/uslack.config";
            var path = IOHelper.MapPath(configFileLocation);
            File.WriteAllText(path, @"{
""token"":""12345"",
""channel"":""ulsackchannel"",
""sections"":[
    
        {""alias"":""d1"",
        ""parameters"":[
            {""alias"": ""al1"",
            ""value"": ""val1""}
        ]},
        {""alias"":""d1"",
        ""parameters"":[
            {""alias"": ""al2"",
            ""value"": ""val2""},
            {""alias"": ""al3"",
            ""value"": ""val3""}
        ]} 
    ]
}");
        }

        [Test]
        public void PopulatePropertiesFromFile()
        {
            var sut = new ConfigurationService();
            sut.Initialize();

            Assert.That(sut.AppConfiguration.Token, Is.EqualTo("12345"));
            Assert.That(sut.AppConfiguration.SlackChannel, Is.EqualTo("ulsackchannel"));

            Assert.That(sut.AppConfiguration.Sections, Has.Count.EqualTo(2));
        }
    }
}
