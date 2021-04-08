[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $TelegramBotToken
)

if ([string]::IsNullOrEmpty($TelegramBotToken))
{
    Write-Host "No Bot Token is passed by -TelegramBotToken parameter" -ForegroundColor Red
    exit
}

$BotTokenEnvironmentVariableName = "TelegramBotConfiguration__TelegramBotToken"

setx $BotTokenEnvironmentVariableName $TelegramBotToken /M
