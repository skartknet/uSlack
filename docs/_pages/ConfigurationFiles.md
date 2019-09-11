---
title: Configuration files
permalink: /configuring/files/
---

All the uSlack configuration files are stored in the `~/App_Plugins/uSlack/config` folder. Here you'll find a collection of `.json` files and one `uslack.config` file.

## uSlack config file
The 'uSlack.config' file stores the settings selected from you Umbraco backoffice. Here you'll see those events and sections configured in your system and the value whether they are enable or disable for a specific configuration section.
To modify when uSlack is sending a message for a especific event you can use the dashboard in the Umbraco backoffice or you can modify the values directly in this file.

## Messages templates
The collection of `.json` files are the configured messages for each specific event. The name of the file follow the convention `service_handler.json` name.

## uSlack default configuration
The configuration shipped with uSlack already includes event handlers for the most useful events that Umbraco has. You can configure the messages sent for each of these events.

To locate the right file to edit you can use the following table:

| Service/uSlack section | Event/uSlack handler | uSlack section | uSlack handler | filename |
| --------| ----- | -------------- | -------------- | -------- |
| ContentService | published | contentService_published.json |
| ContentService | unpublished | contentService_unpublished.json |
| ContentService | trashed | contentService_trashed.json |
| ContentService | sentToPublish | contentService_sentToPublish.json |
| ContentService | rolledBack | contentService_rolledBack.json |
| ContentService | moved |  contentService_moved.json |
| ContentService | deleted | contentService_deleted.json |
| MediaService | saved | mediaService_saved.json |
| MediaService | trashed | mediaService_trashed.json |
| MediaService | moved |  mediaService_moved.json |
| MediaService | deleted | mediaService_deleted.json |
| MemberService | saved | memberService_saved.json |
| MemberService | deleted | memberService_deleted.json |
| UserService | deletedUser | userService_deletedUser.json |
| UserService | deletedUserGroup | userService_deletedUserGroup.json |

You can see the full list of available Umbraco events [here](https://our.umbraco.com/documentation/Reference/Events/).



