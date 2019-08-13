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
            var token = "xoxp-656657692176-645232876739-707926362085-36c540618998851513c7122bbc58dd8d";
            var channel = "CKCEGGARM";
            var blocks = @"[
	{
		'type': 'actions',
        'block_id':'content',
        'elements': [
			{
				'type': 'button',
				'text': {
					'type': 'plain_text',
					'text': 'Farmhouse',
					'emoji': true

                },
				'value': 'click_me_123',
                'action_id' : 'unpublish'
			}
		]
	}
]";

            try{
            var client = new USlackExtendedSlackTaskClient(token);
            await client.PostMessageOnlyBlocksAsync(channel, "test1", blocks);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
