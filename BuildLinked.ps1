# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/SMTVVAlwaysFusionAccident/*" -Force -Recurse
dotnet publish "./SMTVVAlwaysFusionAccident.csproj" -c Release -o "$env:RELOADEDIIMODS/SMTVVAlwaysFusionAccident" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location