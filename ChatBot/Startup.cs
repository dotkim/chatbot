using System;
using System.Net.Http.Headers;
using Chatbot;
using ChatBot.Client;
using ChatBot.Features;
using ChatBot.Services;
using ChatBot.Types;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatBot;

public class Startup(IConfiguration configuration)
{
  private readonly IConfiguration _configuration = configuration;

  public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
  {
    // Register configuration objects
    services.Configure<DiscordSettings>(_configuration.GetSection("DiscordSettings"));
    services.Configure<ApiSettings>(_configuration.GetSection("ApiSettings"));
    services.Configure<ProcessingSettings>(_configuration.GetSection("Processing"));

    // Discord and bot services
    services.AddSingleton<DiscordSocketConfig>(_ => new DiscordSocketConfig
    {
      GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.Guilds | GatewayIntents.GuildMessages
    });

    services.AddSingleton<DiscordSocketClient>();

    services.AddSingleton<InteractionService>(provider =>
    {
      var client = provider.GetRequiredService<DiscordSocketClient>();
      return new InteractionService(client);
    });

    services.AddSingleton<CommandService>();
    services.AddSingleton<CommandHandlingService>();
    services.AddSingleton<DiscordLoggingService>();

    // API client
    services.AddHttpClient<ApiClient>((serviceProvider, client) =>
    {
      var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
      client.BaseAddress = new Uri(apiSettings.BaseUrl);
      if (!string.IsNullOrEmpty(apiSettings.Username) && !string.IsNullOrEmpty(apiSettings.Password))
      {
        var credentials = $"{apiSettings.Username}:{apiSettings.Password}";
        var b64credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", b64credentials);
      }
    });

    // Features
    services.AddSingleton<AttachmentService>();
    services.AddSingleton<ImageService>();
    services.AddSingleton<AudioService>();
    services.AddSingleton<VideoService>();
    services.AddSingleton<KeywordService>();

    // Hosted service for bot lifetime management
    services.AddHostedService<BotHostedService>();
  }

  public void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
  {
    logging.ClearProviders();
    logging.AddProvider(new ChatbotLoggerProvider());
  }
}