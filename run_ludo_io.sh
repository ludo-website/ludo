#!/bin/sh
apk update && apk add jq
jq --arg DATABASE "$DATABASE" '.ConnectionStrings.WebAppDatabase = $DATABASE' appsettings.json > tmp
mv tmp appsettings.json
dotnet Ludo.Io.dll
