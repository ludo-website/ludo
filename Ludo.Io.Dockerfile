FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
COPY backend/ /backend/
WORKDIR /backend
RUN dotnet publish Ludo.Io/Ludo.Io.csproj -c Debug -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
COPY --from=build /app /app
COPY run_ludo_io.sh /app/
WORKDIR /app
EXPOSE $ASPNETCORE_HTTP_PORTS
ENTRYPOINT ["sh", "run_ludo_io.sh"]
