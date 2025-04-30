using System.Threading;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Services;
using ChatBot.Types;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ChatBot;

public class Program
{
  // Runs a method to prevent the entire app from exiting on an error.
  public static async Task Main(string[] args)
  {
    // First init, check config incase its missing.
    Initialize.Start();

    //Load application config
    ConfigurationLoader configLoader = new();
    Configuration appConfig = configLoader.LoadConfig<Configuration>();

    var socketConfig = new DiscordSocketConfig()
    {
      GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    };

    DiscordSocketClient client = new(socketConfig);
    CommandService command = new();

    var commandHandlingService = new CommandHandlingService(client, command);
    await commandHandlingService.InstallCommandsAsync();

    _ = new LoggingService(client, command);

    await client.LoginAsync(TokenType.Bot, appConfig.Token);
    await client.StartAsync();
    await Task.Delay(Timeout.Infinite);
  }
}
