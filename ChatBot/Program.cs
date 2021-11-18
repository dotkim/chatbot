using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
    
    static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
      // First init, check config incase its missing.
      Initialize.Start();

      using (var services = ConfigureServices())
      {
        var loader = new ConfigurationLoader();
        _config = loader.LoadConfig<Configuration>();

        var client = services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;
        services.GetRequiredService<CommandService>().Log += LogAsync;

        await client.LoginAsync(TokenType.Bot, _config.Token);
        await client.StartAsync();

        await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

        await Task.Delay(Timeout.Infinite);
      }
    }

    private Task LogAsync(LogMessage log)
    {
      Console.WriteLine(log.ToString());

      return Task.CompletedTask;
    }

    private ServiceProvider ConfigureServices()
    {
      return new ServiceCollection()
          .AddSingleton<DiscordSocketClient>()
          .AddSingleton<CommandService>()
          .AddSingleton<CommandHandlingService>()
          .BuildServiceProvider();
    }
  }
}
