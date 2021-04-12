# StudentNotifierApp

Student Notifier App is currently composed only of a [Telegram Bot](https://core.telegram.org/bots) which enables users to register/unregister (subscribe/unsubsrcribe) for news notifications.
The registering/unregistering of users is done by sending the */start* and */stop* commands to specific telegram bot.

The news notifications are received on _**/api/NewsNotifications**_ endpoint, after the bot has been started.
When a new news has been received on the endpoint, the news notifications will be broadcast to all registered users.

## Creating a Telegram Bot
In order to use this app, you should have a Telegram Bot set up.

For detailed steps on how to setup a Telegram Bot, consult the Telegram Bot [documentation](https://core.telegram.org/bots#3-how-do-i-create-a-bot)

## Configuration Setup

The `TelegramBot\appsettings.json` file contains the _**"UserMessageReplyConfiguration"**_ subfields, where you can specify the supported messages:

- NewsNotificationText
- StartCommandReplyText
- StopCommandReplyText
- UnsupportedCommandReplyText
- UnregisteredUserSentMessageText

## Installation
Once, your Telegram Bot is ready, you can start using this app. The following step will install the app as a Windows Service:

1. Open Powershell as an administrator (in order to install the Windows Service)
2. Navigate to the location of `Scripts` folder of this solution
3. Execute the `Register-BotToken.ps1` script and provide the `-TelegramBotToken` parameter followed by the Token of your Telegram Bot
   - For more information about your bot's token see the official [documentation](https://core.telegram.org/bots/api#authorizing-your-bot)
   - _"SUCCESS: Specified value was saved."_ message should be displayed
4. Execute the `Install-EnLitenTelegramBot.ps1` script
   - An optional `-ServiceName` parameter can be provided which set the service name for the app.
   - Otherwise, the default _**"EnLitenTelegramBot"**_ will be used
   - _"Finished installing Telegram Bot as windows service"_ message should be displayed
5. Execute the `Start-EnLitenTelegramBot.ps1` script
   - An optional `-ServiceName` parameter can be provided which set the service name for the app.
   - Otherwise, the default _**"EnLitenTelegramBot"**_ will be used
6. Use your Telegram desktop or smartphone app to register/unregister for messages
7. Send messages of format:
   ```JSON
    {
        "id": 1,
        "title": "This is the news title",
        "url": "https://github.com"
    }
   ```
   to endpoint like `http://{{Url}}/api/NewsNotifications`, so the registered users would be notified.

## Uninstallation

In order to stop and uninstall the started telegram bot, execute the following commands:

1. Open Powershell as an administrator (in order to install the Windows Service)
2. Navigate to the location of `Scripts` folder of this solution
3. Execute the `Stop-EnLitenTelegramBot.ps1` script
   - An optional `-ServiceName` parameter can be provided which set the service name for the app.
   - Otherwise, the default _**"EnLitenTelegramBot"**_ will be used
4. Execute the `Uninstall-EnLitenTelegramBot.ps1` script
   - An optional `-ServiceName` parameter can be provided which set the service name for the app.
   - Otherwise, the default _**"EnLitenTelegramBot"**_ will be used
   - _"\[SC\] DeleteService SUCCESS"_ message should be displayed
5. Execute the `Unregister-BotToken.ps1` script
   - _"The operation completed successfully."_ message should be displayed

## Development

In order to further use the source code and contribute to developing this Telegram Bot, 
you should setup your _user-secrets_ with the Token of your Telegram Bot as per Microsoft 
documentation for [user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows#how-the-secret-manager-tool-works)