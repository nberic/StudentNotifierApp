
$BotTokenEnvironmentVariableName = "TelegramBotConfiguration__TelegramBotToken"

reg delete "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /F /V $BotTokenEnvironmentVariableName
