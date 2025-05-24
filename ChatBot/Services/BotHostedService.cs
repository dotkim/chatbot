using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.Types;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ChatBot.Services;

public class BotHostedService : IHostedService
{
  private readonly IServiceProvider _services;
  private readonly IHostEnvironment _environment;
  private readonly DiscordSettings _config;
  private readonly DiscordSocketClient _client;
  private readonly CommandService _commands;
  private readonly CommandHandlingService _commandHandlingService;
  private readonly InteractionService _interactionService;
  private readonly DiscordLoggingService _loggingService;

  public BotHostedService(
      IServiceProvider services,
      IHostEnvironment environment,
      IOptions<DiscordSettings> config,
      DiscordSocketClient client,
      CommandService commands,
      CommandHandlingService commandHandlingService,
      InteractionService interactionService,
      DiscordLoggingService loggingService
  )
  {
    _services = services;
    _environment = environment;
    _config = config.Value;
    _client = client;
    _commands = commands;
    _commandHandlingService = commandHandlingService;
    _interactionService = interactionService;
    _loggingService = loggingService;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await _commandHandlingService.InstallCommandsAsync();
    await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

    _client.InteractionCreated += async interaction =>
    {
      var ctx = new SocketInteractionContext(_client, interaction);
      await _interactionService.ExecuteCommandAsync(ctx, _services);
    };

    _client.Ready += async () =>
    {
      if (_environment.IsProduction())
      {
        // Register commands globally in production
        await _interactionService.RegisterCommandsGloballyAsync();
      }
      else
      {
        // Register commands to a specific guild in development
        await _interactionService.RegisterCommandsToGuildAsync(_config.TestGuildId);
      }
    };

    await _client.LoginAsync(TokenType.Bot, _config.Token);
    await _client.StartAsync();
  }

  public async Task StopAsync(CancellationToken cancellationToken)
  {
    await _client.LogoutAsync();
    await _client.StopAsync();
  }
}