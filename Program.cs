using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using ChatBot.Services;
using ChatBot.Types;
using ChatBot.Libraries;

namespace ChatBot
{
  class Program
  {
    static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
      using (var services = ConfigureServices())
      {
        var loader = new ConfigurationLoader();
        Configuration config = loader.LoadConfig();

        var client = services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;
        services.GetRequiredService<CommandService>().Log += LogAsync;

        await client.LoginAsync(TokenType.Bot, config.Token);
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
          .AddSingleton<HttpClient>()
          .AddSingleton<ImageService>()
          .AddSingleton<KeywordService>()
          .BuildServiceProvider();
    }
  }
}
