# chatbot

Beep boop

This Chatbot requires that you have an [API](https://github.com/dotkim/api) running as a backend.

This bot is made to store and serve images, videos, audio and keywords(text). While in a guild it will check every message for file content and store it. It also reacts to keywords to store and serve text.

## Installing

Currently, the only way to install this is to build it yourself, i do this with Docker and will provide a way to do it with that.

First clone the repo:

```
git clone https://github.com/dotkim/chatbot.git
cd chatbot
```

Build the docker image (NOTE: You might want to configure the chatbot first, the config gets pulled into the image.):

```
docker build -t chatbot .
```

To run the chatbot:

```
docker run -d chatbot
```

## Config

You will need to create a `Configuration.xml` file in the ChatBot/config directory of the repository. Here is a template:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Token>discord token</Token>
  <AvoidDupeCount>100</AvoidDupeCount>
  <BaseUrl>the base api url</BaseUrl>
  <StaticUrl>the static url from the api</StaticUrl>
  <Username>api user</Username>
  <Password>api pw</Password>
</Configuration>
```
