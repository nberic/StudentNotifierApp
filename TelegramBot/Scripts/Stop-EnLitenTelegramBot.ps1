
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

Stop-Service -Name $ServiceName
