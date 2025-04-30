using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ChatBot.Services;

public class CommandHandlingService
{
  private readonly CommandService _commands;
  private readonly DiscordSocketClient _client;

  public CommandHandlingService(DiscordSocketClient client, CommandService commands)
  {
    _commands = commands;
    _client = client;
  }

  public async Task InstallCommandsAsync()
  {
    _commands.CommandExecuted += CommandExecutedAsync;
    _client.MessageReceived += MessageReceivedAsync;
    _client.MessageReceived += CheckMessageForKeywordReceivedAsync;
    await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
  }

  public async Task CheckMessageForKeywordReceivedAsync(SocketMessage messageParam)
  {
    var message = messageParam as SocketUserMessage;
    if (message.Source != MessageSource.User) return;
    if (message == null) return;

    var context = new SocketCommandContext(_client, message);
    var argPos = 0;
    if (!message.HasCharPrefix('!', ref argPos)) return;

    var keywords = await KeywordService.GetAllNamesAsync(context.Guild.Id);
    string keywordName = message.Content.Split(" ")[0].Substring(1);

    if (!keywords.Contains(keywordName.ToUpper())) return;

    var getKeywordResult = await KeywordService.GetAsync(keywordName, context.Guild.Id);

    await context.Channel.SendMessageAsync(getKeywordResult);
  }

  public async Task MessageReceivedAsync(SocketMessage messageParam)
  {
    var message = messageParam as SocketUserMessage;
    if (message.Source != MessageSource.User) return;
    if (message == null) return;

    var context = new SocketCommandContext(_client, message);
    AttachmentService.CheckAndFetchAttachment(context);

    var argPos = 0;
    if (!message.HasCharPrefix('!', ref argPos)) return;

    await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
  }

  public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
  {
    if (!command.IsSpecified)
      return;

    if (result.IsSuccess)
      return;

    await context.Channel.SendMessageAsync($"error: {result}");
  }
}
