---
permalink: /extending/sections
title: Extending
---

You can add new event listener that will respond to other events happening in services not included with uSlack.

Extending the configuration involves several steps.

## Registering new sections
A Configuration Section includes the list of all available events that uSlack can listen to.
The list is made of sections that contain a collection of event handlers.

You can add new sections an event handlers to uSlack with a bit of code.

The configured sections and event handlers will be automatically displayed in the backoffice and will allow administrators to enable or disable whether a handler should respond to an event or not.

### Adding new Configuration Sections
A configuration section is a group of event handlers. The sections shipped in uSlack are grouped by Umbraco services: content, media, users and members

To add a new section you have to create a class that is decorated with the `SectionHandler` attribute. This attribute takes an alias. This alias will be used to create the configuration file of the enabled handlers.

```csharp 
[SectionHandler("contentService")]
public class MyHandlers
{
}
```

### Adding new event handlers
Each section will host the different event handlers that will listen for any configured events.

To indicate that a method is a uSlack event handler you have to decorate the method with the `EventHandler` attribute. 

```csharp
[SectionHandler("contentService")]
public class MyHandlers
{
    [EventHandler("published", true)]
    public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
    {
        //your code here
    }
}
```

This attribute takes an alias. This alias will be used to create the configuration file of the enabled handlers. You can also specify the default value of the switch when displayed in the backoffice.

## Attaching your event handlers.
Once you have created the handlers you have to attach them to the right events so they are fired up and send the messages to Slack. To do this you have to 'compose' your configuration as explained in the [Umbraco documentation](https://our.umbraco.com/documentation/Implementation/Composing/) 

The following is an example of such configuration:

```csharp
public class InitMyConfig : IComponent
{
    private readonly ContentHandlers _myHandlers

    public InitUSlack(ContentHandlers myHandlers)
    {
        _myHandlers = myHandlers;
    }

    // initialize: runs once when Umbraco starts
    public void Initialize()
    {
        ContentService.Published += _myHandlers.ContentService_Published;              
    }
       
    // terminate: runs once when Umbraco stops
    public void Terminate()
    {
        ContentService.Published -= _myHandlers.ContentService_Published;              
    }
}

[RuntimeLevel(MinLevel = RuntimeLevel.Run)]
public class MyComposer : ComponentComposer<InitMyConfig>
{
    // nothing needed to be done here!
}
```

### Dependency injection
In the previous example we are injecting the MyHandler class into the constructor using dependency injection(CD). To use CD you have to register your types as explained in the [Umbraco documentation](https://our.umbraco.com/documentation/Reference/using-ioc).

You have an example about how to register your types below:

```csharp
public class TypesRegistration : IUserComposer
{
    public void Compose(Composition composition)
    {
        composition.Register(typeof(MyHandlers));          
    }
}
```
:::

## Sending messages to Slack
At this point we have a bunch of event handlers attached to some events, but they are not sending messages to our Slack channels yet.

To be able to send messages you have to use the `SlackService` class.

> **TIP**
The SlackService class is already registered in the Umbraco DI container so you can use it through constructor injection using the `IMessageService` interface.

```csharp

[SectionHandler("contentService")]
public class ContentHandlers : EventHandlerBase
{
    private readonly IMessageService _messagingService;

    public ContentHandlers(IMessageService messagingService)
    {
        _messagingService = messagingService;
    }

       [EventHandler("published", true)]
        public void ContentService_Published(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentPublishedEventArgs e)
        {
            foreach (var item in e.PublishedEntities)
            {
                var properties = new PropertiesDictionary(item);
                var publisher = Current.Services.UserService.GetUserById(item.PublisherId.GetValueOrDefault());
                if (publisher != null) properties.Add("publisher", publisher.Name);

                AsyncUtil.RunSync(() => _messagingService.SendMessageAsync("contentService", "published", properties));
            }
        }
}

```

As you can see n the previous example we iterate over all the entities in `e.PublishedEntities`.
For each entity, we use the `PropertiesDictionary` to populate a collection of default properties to be used in our messages using placeholders. This dictionary adds the name, id, and the custom properties available in the entity.
In addition to the default values we are adding the name of the using causing this publishing event.
Then we call the `SendMessageAsync` method, passing the section and event aliases and our generated properties collection.
This method will take care of finding the right template, replacing the placehlders and sending the message to the available configurations.

As you can see, we are using the `AsyncUtil.RunSync` method to allow us to run this method synchronously. This is because we can't run async methods in the Umbraco event handlers.