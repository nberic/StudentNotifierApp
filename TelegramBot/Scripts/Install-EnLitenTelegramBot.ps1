
Set-Location -Path ..\

if (Test-Path -Path .\Publish)
{
    Remove-Item -Path .\Publish -Recurse
}

# dotnet publish -c Debug -f net5.0 -r win10-x64 .\TelegramBot.csproj -o .\Publish\ --self-contained
dotnet publish -o .\Publish

$CurrentDirectoryFullName = (Get-Location).Path

sc.exe create EnLitenTelegramBot binPath=$CurrentDirectoryFullName\Publish\TelegramBot.exe
sc.exe config EnLitenTelegramBot start=demand

Set-Location -Path .\Scripts