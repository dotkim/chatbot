FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /chatbot
COPY --from=build-env /app/out .
COPY Configuration.xml .
ENTRYPOINT ["dotnet", "ChatBot.dll"]
