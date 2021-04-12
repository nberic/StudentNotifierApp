
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

Write-Host "Verifying nssm installation" -ForegroundColor Green

$ExpectedNssmPath = "\Tools\nssm-2.24\win64\nssm.exe"
$SolutionPath = (Get-Item ..\).FullName

if (-Not (Test-Path -Path $SolutionPath\$ExpectedNssmPath))
{
    Write-Host "Nssm not found at $SolutionPath\$ExpectedNssmPath" -ForegroundColor Red
    Write-Host "Copy it on that location" -ForegroundColor Red
    exit
}

Write-Host "Successfully verified nssm installation" -ForegroundColor Green

Set-Location -Path ..\TelegramBot

Write-Host "Starting prepublish cleanup" -ForegroundColor Green

if (Test-Path -Path ..\Publish)
{
    Remove-Item -Path ..\Publish -Recurse
}

Write-Host "Finished prepublish cleanup" -ForegroundColor Green

Write-Host "Publishing Telegram Bot" -ForegroundColor Green

dotnet publish -o ..\Publish\TelegramBot\

Write-Host "Finished publishing Telegram Bot" -ForegroundColor Green

$ServiceExePath = Get-ChildItem ..\Publish\TelegramBot\TelegramBot.exe

Set-Location -Path ..\Tools

Write-Host "Installing Telegram Bot as windows service" -ForegroundColor Green

.\nssm-2.24\win64\nssm.exe install $ServiceName $ServiceExePath.FullName
.\nssm-2.24\win64\nssm.exe set $ServiceName AppDirectory $ServiceExePath.DirectoryName
.\nssm-2.24\win64\nssm.exe set $ServiceName Start SERVICE_DEMAND_START

Write-Host "Finished installing Telegram Bot as windows service" -ForegroundColor Green

Set-Location -Path ..\Scripts
