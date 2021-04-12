

[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $ServiceName
)

if ([string]::IsNullOrEmpty($ServiceName))
{
    $ServiceName = "EnLitenTelegramBot"
}

Set-Service -Name $ServiceName -Status Stopped
