
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

$ExpectedNssmPath = "Tools\nssm-2.24\win64\nssm.exe"
$SolutionPath = (Get-Item ..\).FullName

if (-Not (Test-Path -Path $SolutionPath\$ExpectedNssmPath))
{
    Write-Host "Nssm not found at $SolutionPath\$ExpectedNssmPath" -ForegroundColor Red
    Write-Host "Please install it and extract on that location" -ForegroundColor Red
    exit
}

Set-Location -Path ..\TelegramBot

if (Test-Path -Path ..\Publish)
{
    Remove-Item -Path ..\Publish -Recurse
}

dotnet publish -o ..\Publish\TelegramBot\

$ServiceExePath = Get-ChildItem ..\Publish\TelegramBot\TelegramBot.exe

Set-Location -Path ..\Tools

.\nssm-2.24\win64\nssm.exe install $ServiceName $ServiceExePath.FullName
.\nssm-2.24\win64\nssm.exe set $ServiceName AppDirectory $ServiceExePath.DirectoryName
.\nssm-2.24\win64\nssm.exe set $ServiceName Start SERVICE_DEMAND_START

Set-Location -Path ..\Scripts
