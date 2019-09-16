---
title: Using the uSlack dashboard
permalink: /using-dashboard/
---

At this point we have already installed uSlack, created an app in Slack and installed it into our Slack Workspace. Now we are ready to link our Umbraco site with Slack using uSlack and start getting notifications!


## Dashboard
First, go to your Umbraco backoffice and navigate to the uSlack section.

Here you will at least one configuration section. Each configuration section allows to set up different values groups. For instance you could create a section that sends all the Content events to a Slack channel and all the Members events to a different channel.

![dashboard](https://raw.githubusercontent.com/skartknet/uSlack/master/docs/images/dashboard01.png)

### Options
**Configuration name:** Give a name to your section

**User groups:** Select the group/s a user has to belong to trigger the message. For instance, don't send a message when a page has been deleted if who did it was a user in the _admin_ group.

**Channel:** Select the channel where uSlack has to send the messages.

#### Events 
Following these options you will find all the available events registered with uSlack. Turn the switch on/off to your requirements.