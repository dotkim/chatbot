using System;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Services;
using ChatBot.Types;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ChatBot
{
  class Program
  {
    private Configuration _config;
    private DiscordSocketClient _client;
    private CommandService _commands;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
      // First init, check config incase its missing.
      Initialize.Start();

      //Load configs
      var loader = new ConfigurationLoader();
      _config = loader.LoadConfig<Configuration>();

      var discordClientConfig = new DiscordSocketConfig()
      {
        GatewayIntents = GatewayIntents.AllUnprivileged,
        LogLevel = LogSeverity.Info
      };

      var commandServiceConfig = new CommandServiceConfig()
      {
        LogLevel = LogSeverity.Info
      };

      // Init services
      _client = new DiscordSocketClient(discordClientConfig);
      _commands = new CommandService(commandServiceConfig);

      _client.Log += Log;
      _commands.Log += Log;

      await _client.LoginAsync(TokenType.Bot, _config.Token);
      await _client.StartAsync();

      var commandHandler = new CommandHandlingService(_client, _commands);
      await commandHandler.InstallCommandsAsync();

      await Task.Delay(Timeout.Infinite);
    }

    private Task Log(LogMessage msg)
    {
      Console.WriteLine(msg.ToString());
      return Task.CompletedTask;
    }
  }
}
