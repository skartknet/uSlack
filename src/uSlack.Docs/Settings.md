# Settings

All the uSlack settings are stored in `~/App_Plugins/uSlack/config`. Here you'll find a collection of `.json` files and one `uslack.config` file.

## uSlack config file
The 'uSlack.config' stores the settings selected from you Umbraco backoffice. Here you'll see those events and sections configured in your system and the value whether they are enable or disable for a specific configuration section. This file contains an array of configurations as you can have multiple configurations, i.e with different tokens for different workspaces or channels.

## Messages templates
The collection of `.json` files are the configured messages for each specific event. The name of the file is by convention the alias of the configuration section followed by underscore and the alias of the handler alias.



## uSlack default configuration
The configuration shipped with uSlack already includes event handlers for the most useful events that Umbraco has. You can configure the messages sent for each of these events. To localise the right file to edit you can use the following table:

| Service | Event | uSlack section | uSlack handler | filename |
| --------| ----- | -------------- | -------------- | -------- |
| ContentService | published | ContentService | published | contentService_published.json |
| ContentService | unpublished | ContentService | unpublished | contentService_unpublished.json |
| ContentService | trashed | ContentService | trashed | contentService_trashed.json |
| ContentService | sentToPublish | ContentService | sentToPublish | contentService_sentToPublish.json |
| ContentService | rolledBack | ContentService | rolledBack | contentService_rolledBack.json |
| ContentService | moved | ContentService | moved | contentService_moved.json |
| ContentService | deleted | ContentService | deleted | contentService_deleted.json |
| MediaService | saved | MediaService | saved | mediaService_saved.json |
| MediaService | trashed | MediaService | trashed | mediaService_trashed.json |
| MediaService | moved | MediaService | moved | mediaService_moved.json |
| MediaService | deleted | MediaService | deleted | mediaService_deleted.json |
| MemberService | saved | MemberService | saved | memberService_saved.json |
| MemberService | deleted | MemberService | deleted | memberService_deleted.json |
| UserService | deletedUser | UserService | deletedUser | userService_deletedUser.json |
| UserService | deletedUserGroup | UserService | deletedUserGroup | userService_deletedUserGroup.json |


You can see the full list of available Umbraco events [here](https://our.umbraco.com/documentation/Reference/Events/).


::: tip

As you can see, the convention we have followed to name our sections and handlers are of the type `service_event.json`.

:::

## Modifying the messages
You can modify the messages sent to your Slack. The JSON included in the configuration files corresponds with that generated b the Slack's [Block Kit Builder](https://api.slack.com/block-kit)

### Placeholders
You can use placholders in your messages that will be replaced by the entity property which alias is included in the placeholder when the messages are sent. The placeholders have the structure `@{property}`.
For example, `@{Name}` will be replace by the Name property and `@{Id}` by the Id property if you are sending an Umbraco `IEntity`.

::: warning
uSlack currently doesn't support nested properties in complex objects. You properties must live in the first level of your class.