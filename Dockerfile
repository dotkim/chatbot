FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore

RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine-composite AS runtime

# Because this is using a alpine composite image, it will error out
#  as the Discord.Net package REQUIRES the en-US culture.
# Running this command adds this to the image we are creating.
# https://github.com/discord-net/Discord.Net/issues/2704
RUN apk add --no-cache icu-libs

WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "ChatBot.dll"]
