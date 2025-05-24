# chatbot

Beep boop

This Chatbot requires that you have an API running as a backend. Currently it is NOT public.

This bot is made to store and serve images, videos, audio and keywords(text). While in a guild it will check every message for file content and store it. It also reacts to keywords to store and serve text.

# Installing

Currently, the only way to install this is to build it yourself, i do this with Docker and will provide a way to do it with that.

First clone the repo:

```bash
mkdir -p ~/discordbot
cd ~/discordbot
git clone https://github.com/dotkim/chatbot.git
```

## Setting up the compose directory

Change this to suit however youd like.

```bash
mkdir -p ~/compose/chatbot/config
touch ~/compose/chatbot/docker-compose.yml
cp ~/discordbot/ChatBot/appsettings.json ~/compose/chatbot/config/appsettings.json
```

and paste this into the `docker-compose.yml` file.

```yaml
services:
  bot:
    build: ../../discordbot/chatbot
    image: chatbot:latest
    pull_policy: never
    environment:
      - DOTNET_NOLOGO=1 #Remove logos and stuff from prod.
      - DOTNET_CLI_TELEMETRY_OPTOUT=1 #Dont send usage data.
      - DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
      - DOTNET_ENVIRONMENT=Production
      - ASPNETCORE_ENVIRONMENT=Production
    volumes:
      - ./config/appsettings.json:/app/appsettings.json:ro
    restart: unless-stopped
```

## Config

Open the `appsettings.json` file and set the settings

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Information",
      "ChatBot.Services.CommandHandlingService": "Information"
    }
  },
  "DiscordSettings": {
    "Token": "YOUR_DISCORD_BOT_TOKEN_HERE",
    "TestGuildId": "Dont need to fill this"
  },
  "ApiSettings": {
    "BaseUrl": "the base url for the API",
    "Username": "",
    "Password": "",
    "StaticUrl": "the static URL to redirect to"
  },
  "Processing": {
    "AvoidDupeCount": 10
  }
}
```
