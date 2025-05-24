using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace ChatBot.Services;

public class DiscordLoggingService
{
  private readonly ILogger<DiscordLoggingService> _logger;

  public DiscordLoggingService(
    DiscordSocketClient client,
    CommandService command,
    InteractionService interaction,
    ILogger<DiscordLoggingService> logger)
  {
    _logger = logger;

    client.Log += LogAsync;
    command.Log += LogAsync;
    interaction.Log += LogAsync;
  }

  private Task LogAsync(LogMessage message)
  {
    if (message.Exception is CommandException cmdException)
    {
      var msg = $"[Command/{message.Severity}] {cmdException.Command.Aliases.First()} failed to execute in {cmdException.Context.Channel}.";
      _logger.LogError(cmdException, msg);
    }
    else
    {
      var logLevel = ConvertSeverity(message.Severity);
      _logger.Log(logLevel, message.Exception, "[General/{Severity}] {Message}", message.Severity, message.Message);
    }

    return Task.CompletedTask;
  }

  private LogLevel ConvertSeverity(LogSeverity severity) => severity switch
  {
    LogSeverity.Critical => LogLevel.Critical,
    LogSeverity.Error => LogLevel.Error,
    LogSeverity.Warning => LogLevel.Warning,
    LogSeverity.Info => LogLevel.Information,
    LogSeverity.Verbose => LogLevel.Debug,
    LogSeverity.Debug => LogLevel.Trace,
    _ => LogLevel.Information
  };
}