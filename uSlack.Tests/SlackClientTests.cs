using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uSlack.Configuration;
using uSlack.Services;
using uSlack.Tests.Data;

namespace uSlack.Tests
{
    [TestFixture]
    public class SlackServiceTests
    {
        [Test]
        public async Task SendMessage()
        {
            var token = "xoxp-656657692176-645232876739-707926362085-36c540618998851513c7122bbc58dd8d";
            var channel = "CKCEGGARM";
            var text = "testmsg";
            var blocks = JsonConvert.DeserializeObject<Block[]>(TestingData.BasicMessage);

            try
            {
                var config = new Mock<IConfiguration>();
                config.Setup(c => c.AppSettings).Returns(new AppSettings
                {
                    Token = token,
                    ConfigurationGroups = new ConfigurationGroup[]
                    {
                        new ConfigurationGroup
                        {
                            SlackChannel = channel,
                            Sections = new System.Collections.Generic.Dictionary<string, ConfigSection>()
                            {
                                { "contentservice", new ConfigSection
                                    {
                                        SectionHandlers = new Dictionary<string, object>()
                                        {
                                            { "published", true }
                                        }
                                    }
                                }
                            }

                        }
                    }
                });

                config.Setup(c => c.GetMessage(It.IsAny<string>())).Returns(new MessageConfiguration { Text = text, Blocks = blocks });
                var client = new SlackService(config.Object);
                await client.SendMessageAsync("contentservice", "published", null);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
