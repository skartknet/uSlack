using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Configuration;
using uSlack.Services;

namespace uSlack.Tests
{
    [TestFixture]
    public class SlackServiceShould
    {
        private Mock<IMessageService> _messageService;
        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Init()
        {
            SetUpConfigurationMock();
        }


        private void SetUpConfigurationMock()
        {
            _messageService = new Mock<IMessageService>();
            _configuration = new Mock<IConfiguration>();
            var messagesConfig = new Dictionary<string, string>();
            var appConfig = new AppConfiguration();
            appConfig.Token = "xoxp-656657692176-645232876739-658179266944-834090019227aa80b4a9f33d43f615ab";
            appConfig.SlackChannel = "uslack";

            _configuration.Setup(config => config.AppConfiguration).Returns(appConfig);

        }

        [Test]
        public void SendMessage()
        {
            var sut = new SlackService();
            var txt = "this is the text";
            var blocks = @"[
	                        {
		                        ""type"": ""section"",

                                ""text"": {
                                        ""type"": ""mrkdwn"",
			                        ""text"": ""Hello, Assistant to the Regional Manager Dwight! *Michael Scott* wants to know where you'd like to take the Paper Company investors to dinner tonight.""

                                }
                            }
                            ]";

            Assert.DoesNotThrowAsync(async () => await sut.SendMessageAsync(txt, blocks));

        }


        [Test]
        public async Task GetChannels()
        {
            var sut = new SlackService();
            await sut.GetConversationsAsync();
        }
    }
}
