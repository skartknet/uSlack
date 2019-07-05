using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uSlack.Services;

namespace uSlack.Tests
{
    [TestFixture]
    public class SlackClientTests
    {
        [Test]
        public async Task SendMessage()
        {
            var token = "xoxp-656657692176-645232876739-658179266944-834090019227aa80b4a9f33d43f615ab";
            var channel = "CKCEGGARM";
            var blocks = @"[
	{
		'type': 'actions',

        'elements': [
			{
				'type': 'button',
				'text': {
					'type': 'plain_text',
					'text': 'Farmhouse',
					'emoji': true

                },
				'value': 'click_me_123'
			},
			{
				'type': 'button',
				'text': {
					'type': 'plain_text',
					'text': 'Kin Khao',
					'emoji': true
				},
				'value': 'click_me_123'
			},
			{
				'type': 'button',
				'text': {
					'type': 'plain_text',
					'text': 'Ler Ros',
					'emoji': true
				},
				'value': 'click_me_123'
			}
		]
	}
]";
            var client = new USlackExendedSlackTaskClient(token);
            await client.PostMessageOnlyBlocksAsync(channel, "test1", blocks);

        }
    }
}
