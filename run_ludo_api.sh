#!/bin/sh
apk update && apk add jq
jq --arg IO_BASE_ADDRESS "$IO_BASE_ADDRESS" '.ClientConfiguration.BaseAddress = $IO_BASE_ADDRESS' appsettings.json > tmp
mv tmp appsettings.json
dotnet Ludo.Api.dll
