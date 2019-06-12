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
        private Mock<IContentService> _contentService;
        private Mock<AppConfiguration> _configuration;

        private Mock<IMessageService> _messageService;
        private ContentPublishedEventArgs _evArgs;


        [SetUp]
        public void Init()
        {
            SetUpConfigurationMock();

            _contentService = new Mock<IContentService>();

            var contentNode = new Mock<IContent>();
            contentNode.Setup(node => node.Name).Returns("nodeName");

            var evMessages = new EventMessages();
            evMessages.Add(new EventMessage("content", "content saved!"));

            _evArgs = new ContentPublishedEventArgs(new IContent[] { contentNode.Object }, false, evMessages);
        }

        private void SetUpConfigurationMock()
        {
            _messageService = new Mock<IMessageService>();
            _configuration = new Mock<AppConfiguration>();
            var messagesConfig = new MessagesConfiguration();
            messagesConfig.Initialize();
            _configuration.Setup(config => config.Messages).Returns(messagesConfig);
        }

        [Test]
        public void SendIContentMessage()
        {
            var contentHandler = new ContentHandlers(_messageService.Object, _configuration.Object);

            Assert.DoesNotThrow(() => contentHandler.ContentService_Published(_contentService.Object, _evArgs));

        }

    }

}
