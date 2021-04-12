
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

sc delete $ServiceName
