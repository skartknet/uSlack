using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSlack.Configuration;
using uSlack.EventHandlers;

namespace uSlack.Tests
{
    [TestFixture]
    public class ContentHandlerShould
    {
        private Mock<IContentService> contentService;
        private Mock<IAppConfiguration> configuration;

        private Mock<IMessageService> messageService;
        private ContentPublishedEventArgs evArgs;


        [SetUp]
        public void Init()
        {
            SetUpConfigurationMock();

            contentService = new Mock<IContentService>();

            var contentNode = new Mock<IContent>();
            contentNode.Setup(node => node.Name).Returns("nodeName");

            var evMessages = new EventMessages();
            evMessages.Add(new EventMessage("content", "content saved!"));

            evArgs = new ContentPublishedEventArgs(new IContent[] { contentNode.Object }, false, evMessages);
        }

        private void SetUpConfigurationMock()
        {
            messageService = new Mock<IMessageService>();
            configuration = new Mock<IAppConfiguration>();
            var messagesConfig = new MessagesConfiguration();
            messagesConfig.Initialize();
            configuration.Setup(config => config.Messages).Returns(messagesConfig);
        }

      
    }
}
