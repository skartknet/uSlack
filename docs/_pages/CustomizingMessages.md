---
layout: single
permalink: /configuring/messages/
title: Customizing the messages
---

You can modify the messages sent to your Slack. The JSON included in the configuration files corresponds with that generated with the Slack's [Block Kit Builder](https://api.slack.com/block-kit)

## Placeholders
You can use placholders in your messages that will be replaced by the property value. The placeholders have the structure `{propertyAlias}`.
For example, `{name}` will be replaced by the Name property and `{id}` by the Id property.
The properties available for these placeholders are the name, id, createDate, deleteDate (if node has been deleted) and the custom properties available in the entity.
