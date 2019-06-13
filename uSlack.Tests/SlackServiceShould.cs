using Moq;
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
    public class SlackServiceShould
    {
        private Mock<IMessageService> _messageService;
        private Mock<IConfigurationService> _configuration;

        [SetUp]
        public void Init()
        {
            SetUpConfigurationMock();
        }


        private void SetUpConfigurationMock()
        {
            _messageService = new Mock<IMessageService>();
            _configuration = new Mock<IConfigurationService>();
            var messagesConfig = new MessagesConfiguration();
            var appConfig = new UslackConfiguration();
            
            _configuration.Setup(config => config.MessagesConfiguration).Returns(messagesConfig);
            _configuration.Setup(config => config.AppConfiguration).Returns(appConfig);

            _configuration.Setup(config => config.AppConfiguration.Token).Returns("xoxp-656657692176-645232876739-658179266944-834090019227aa80b4a9f33d43f615ab");
            _configuration.Setup(config => config.AppConfiguration.SlackChannel).Returns("uslack");

        }

        [Test]
        public void SendMessage()
        {
            var sut = new SlackService(_configuration.Object);
            var txt = "this is the text";
            var blocks = @"[
	                        {
		                        ""type"": ""section"",

                                ""text"": {
                                        ""type"": ""mrkdwn"",
			                        ""text"": ""Hello, Assistant to the Regional Manager Dwight! *Michael Scott* wants to know where you'd like to take the Paper Company investors to dinner tonight.\n\n *Please select a restaurant:*""

                                }
                            }
                            ]";
            Assert.DoesNotThrowAsync(async () => await sut.SendMessageAsync(txt, blocks));

        }
    }
}
