---
title: Interactivity
permalink: /interactivity/
---

uSlack supports [interactive Slack messsages](https://api.slack.com/interactive-messages)



## Configuring your Slack app for Interactive Messages
In order for your app to talk back to your Umbraco installation, you have to configure a Request URL. This Url is the gateway to your Umbraco site.

Navigate to [Your Apps in Slack](https://api.slack.com/apps), find your uSlack app and click it. Navigate to the _Interactive Components_ section.

![request url](https://raw.githubusercontent.com/skartknet/uSlack/master/docs/images/interactivity-requesturl.png)

In the **Request Url** field you have to enter your domain followed by the path `/umbraco/uslack/interactiveapi/processresponse`

This will send the response to the uSlack endpoint.


## Preconfigured interactive responses
uSlack comes preconfigured with some basic interactivity that can be extended to your needs. For example, the Umbraco published page message has a button to quickly unpublish that page.

Apart of configuring your Slack app with a Request URL (See point above) there's nothing else you need to do to handle this responses.

## Creating your own Interactive Controllers
The controllers that handle Slack responses are called Interactive Controllers. These controllers are based in WebApi controllers but validate the Slack response using a Slack Signature and your app's Signing Secret. You have more infor about this process [here.](https://api.slack.com/docs/verifying-requests-from-slack.)

To create your own controller, inherit from `InteractiveApiControllerBase`. The following is the uSlack code that handles an unpublish content response:

```csharp
using System.Threading.Tasks;
using System.Web.Http;
using uSlack.Interactive;

namespace uSlack.API
{
    public class ContentController : InteractiveApiControllerBase
    {

        public async Task<IHttpActionResult> Unpublish(string src)
        {
            if (int.TryParse(src, out int id))
            {
                var node = Services.ContentService.GetById(id);
                if (node == null) return BadRequest("Node doesn't exist");
                Services.ContentService.Unpublish(node);
            }
            else
            {
                return BadRequest("Value passed is not a valid id. It needs to be an integer.");
            }

            return Ok();
        }
    }
}
```

The `InteractiveApiControllerBase` inherits from `UmbracoApiController` so you have access to all the Umbraco services, context and helpers that that controller provides.

## Creating your interactive message
Now that you have the action that's going to handle your response, you need to add an interactive element to your message. We'll have to configure this element in certain way so it communicates with your newly created Interactive controller and action.

Slack allows you to create different [interactive elements](https://api.slack.com/reference/messaging/interactive-components).

An interactive element has to be included in a layout block. You can create the correct message structure using the [Slack Block Kit](https://api.slack.com/block-kit)


## The interactive routing
Slack sends the reponses for an interactive event to the configured **Request URL** we configured before. There's a uSlack controller there that uses some parameters in the request to find the right Interactive Controller.

The layout block has a `block_id` property that you'll have to use to specify the name of your Interactive controller.

The interactive elements have an `action_id` property that you'll have to use to specify the action name that you want to use to handle your repsonse.

Each interactive element returns a value. This value returns the `value` property  value when it's clicked if it's a button or select if it;s a dropdown. Each interactive element returns a different property name with this value but you don't have to worry about that.

**NOTE**: At this stage uSlack only supports returning strings. If you return a JSON object for example you will have to deserialize it yourself in your Interactive controller.

And example of a message using an interactive element is in the `~/App_Pluging/uSlack/Config/contentService_published.json` file. This message is sent when a 'published' event occur.

```json
{
  "text": "Content item has been published",
  "blocks": [
    {
      "type": "section",
      "block_id": "content", //NAME OF THE CONTROLLER WITHOUT THE 'CONTROLLER' SUFFIX
      "text": {
        "type": "mrkdwn",
        "text": "The page *{name}* has been published."
      },
      "accessory": {
        "type": "button",
        "action_id": "unpublish", //NAME OF THE ACTION METHOD
        "text": {
          "type": "plain_text",
          "text": "Unpublish",
          "emoji": true
        },
        "value": "{id}" //VALUE PASSED TO THE ACTION METHOD
      }
    },
    {
      "type": "context",
      "elements": [
        {
          "type": "mrkdwn",
          "text": "*Publisher:* {publisher}"
        }
      ]
    }
  ]
}
```

You can see in the previous example that we are expecting for uSlack to find a `Content` controller (without the _controller_ suffix), and run a method in it called `Unpublished`. The value passed to our method is the [button element](https://api.slack.com/reference/messaging/block-elements#button) value: {id}

> **NOTE:** The `{id}` is a placeholder that will display the actual page id when the message is sent. you have more info in [Customizing Messages](../configuring/messages/)