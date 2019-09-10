---
permalink: /extending/
layout: single
title: Extending
toc: true
---

You can add new event listener that will respond to other events happening in services not included with uSlack.

Extending the configuration involves several steps.

## Registering components
A configuration object includes the list of all available events that uSlack can listen to.
The list is made of sections that contain the handlers. The configured sections and event handlers will be automatically displayed in the backoffice and will allow administrators to enable or disable whether a handler should respond to an event or not..

### Adding new Configuration Sections
A configuration section is a group of event handlers. This events should be group but some logic. The sections shipped in uSlack are grouped by Umbraco services: content, media, users and members

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
Once you have created the handlers you have to attach them to the right events so they are fired and send the messages to Slack. To do this you have to 'compose' your configuration as explained in the [Umbraco documentation](https://our.umbraco.com/documentation/Implementation/Composing/) 

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
:::tip
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
At this point we have a bunch of enevent handlers attached to some events, but they are not sending messages to our Slack channels yet.

To be able to send messages you have to use the `SlackService` class.

::: tip
The SlackService class is already registered in the Umbraco DI container so you can use it through constructor injection using the `IMessageService` interface.
:::

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
        _messagingService.SendMessage("contentService", "published", e.PublishedEntities);
    }
}

```

The `SendMessage` method accepts the aliases of the section and the handler to find the configuration in the uSlack settings. You also have to pass the `IEntities` that are being processed.

::: tip

All the uSlack handlers manage Umbraco `IEntity` objects but you can create your own Service implemeting the IMessageService<T> interface  class and creating those methods that you need to handle your own special entities.

:::