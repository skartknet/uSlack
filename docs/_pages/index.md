---
# Feel free to add content and custom Front Matter to this file.
# To modify the layout, see https://jekyllrb.com/docs/themes/#overriding-theme-defaults

title: Introduction
layout: single
permalink: /
---

uSlack is a package for Umbraco V8. Umbraco and many other systems trigger different type of events when tasks are performed either by users or automatically. uSlack attachs a variety of handlers that will react to such events and will send messages to a specific Slack workspace and channel.


## Installing

1. Umbraco Package
You can install uSlack as an umbraco package from the Umbraco backoffice.

2. Nuget Package

## Getting Started

Once you have installed uSlack there are a few easy steps you will need to follow to allow uSlack to send messages to your own Slack channel.

### Creating a personal app

To allow uSlack to send messages to your channels you will have to create your private Slack app that will generate the necessary tokens to allow your website to send messages into it.

First navigate and log in to your [Slack](https://slack.com) workspace. Once you are logged in navigate to [Your Apps (https://api.slack.com/apps)](https://api.slack.com/apps) and click _Create an App_.

In the dialog enter a name for your app and select the workspace this app is going to have access to. Click on _Create App_

### Configuring your app

#### Permission scope
Firstly we have to assign different permissions to our app that uSlack will use to send a receive messages from your Slack channels. To do so navigate to the _Oauth & Permissions_ option in the menu on the left. Then scroll down to the _Scopes_ section.

In the dropdown we'll need to add the following permissions:
- channels:read
- chat:write:bot
- commands 

Click _Save Changes_


#### Installing the app
First we need to install our app into the workspace. To do so click on _Install App_ on the left side menu. Then click con the _Install App to Workspace_ button. Click _Install_ on the following screen.
Once the app has installed you'll get an OAuth token. Take note of this because we'll need it when configuring uSlack in Umbraco.

### Configuring uSlack

At this point we are ready to link our Umbraco site with Slack using uSlack. First, go to your Umbraco backoffice and navigate to the uSlack section.