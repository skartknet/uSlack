using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSlack.Tests.Data
{
    internal static class TestingData
    {
        internal static string BasicMessage = @"[
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
    }
}
