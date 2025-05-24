using System;
using System.Reflection;
using System.Threading.Tasks;
using ChatBot.Features;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace ChatBot.Services;

public class CommandHandlingService
{
  private readonly DiscordSocketClient _client;
  private readonly CommandService _commands;
  private readonly AttachmentService _attachmentService;
  private readonly KeywordService _keywordService;
  private readonly IServiceProvider _services;
  private readonly ILogger<CommandHandlingService> _logger;

  public CommandHandlingService(
      DiscordSocketClient client,
      CommandService commands,
      AttachmentService attachmentService,
      KeywordService keywordService,
      IServiceProvider services,
      ILogger<CommandHandlingService> logger
  )
  {
    _client = client;
    _commands = commands;
    _attachmentService = attachmentService;
    _keywordService = keywordService;
    _services = services;
    _logger = logger;
  }

  public async Task InstallCommandsAsync()
  {
    _commands.CommandExecuted += CommandExecutedAsync;
    _client.MessageReceived += MessageReceivedAsync;
    await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
  }

  public async Task MessageReceivedAsync(SocketMessage messageParam)
  {
    var message = messageParam as SocketUserMessage;
    if (message == null) return;
    if (message.Source != MessageSource.User) return;

    var context = new SocketCommandContext(_client, message);

    _logger.LogInformation("Message received from {User} in {Channel} (Guild: {Guild}) at {Timestamp}: {Content}",
        message.Author.Username,
        context.Channel.Name,
        context.Guild != null ? context.Guild.Name : "DM",
        message.Timestamp,
        message.Content);

    await _attachmentService.CheckAttachmentAsync(context);

    var argPos = 0;
    if (!message.HasCharPrefix('!', ref argPos)) return;

    _logger.LogInformation("Command prefix detected in message from {User}: {Content}", message.Author.Username, message.Content);

    string keywordName = message.Content.Split(" ")[0][1..];
    var lowerKeyword = keywordName.ToLower();
    if (!(lowerKeyword is "r" or "a" or "v" or "random" or "aandom" or "vandom"))
    {
      var keywords = await _keywordService.GetAllNamesAsync(context.Guild.Id);
      if (keywords.Contains(keywordName.ToUpper()))
      {
        _logger.LogInformation("Keyword '{Keyword}' matched in guild {Guild}", keywordName, context.Guild.Id);
        var getKeywordResult = await _keywordService.GetAsync(keywordName, context.Guild.Id);
        await context.Channel.SendMessageAsync(getKeywordResult);
        return;
      }
    }

    _logger.LogInformation("Executing command for {User}: {Command}", message.Author.Username, message.Content);

    await _commands.ExecuteAsync(context: context, argPos: argPos, services: _services);
  }

  public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
  {
    if (!command.IsSpecified)
      return;

    if (result.IsSuccess)
      return;

    _logger.LogWarning("Command '{Command}' by {User} failed: {Error}", command.Value.Name, context.User.Username, result.ErrorReason);
    await context.Channel.SendMessageAsync($"error: {result}");
  }
}