using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class AttachmentService
{
  private readonly ApiSettings _apiSettings;
  private readonly AudioService _audioService;
  private readonly ImageService _imageService;
  private readonly VideoService _videoService;
  private readonly ILogger<AttachmentService> _logger;

  public AttachmentService(
      IOptions<ApiSettings> apiSettingsOptions,
      AudioService audioService,
      ImageService imageService,
      VideoService videoService,
      ILogger<AttachmentService> logger)
  {
    _apiSettings = apiSettingsOptions.Value ?? throw new ArgumentNullException(nameof(apiSettingsOptions));
    _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
    _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task CheckAttachmentAsync(SocketCommandContext context)
  {
    if (context?.Message == null)
    {
      _logger.LogWarning("CheckAttachmentAsync called with null context or message.");
      throw new ArgumentNullException(nameof(context));
    }

    var tasks = new List<Task>();

    if (context.Message.Attachments.Count > 0)
    {
      _logger.LogDebug("Message {MessageId} has {AttachmentCount} attachments. Checking attachments...", context.Message.Id, context.Message.Attachments.Count);
      tasks.Add(CheckAttachmentsAsync(context.Guild.Id, context.Message.Author.Id, context.Message.Attachments));
    }

    if (!string.IsNullOrWhiteSpace(context.Message.Content))
    {
      _logger.LogDebug("Message {MessageId} has content. Checking content for URLs...", context.Message.Id);
      tasks.Add(CheckContentAsync(context.Guild.Id, context.Message.Author.Id, context.Message.Id.ToString(), context.Message.Content));
    }

    await Task.WhenAll(tasks);
    _logger.LogInformation("CheckAttachmentAsync completed for message {MessageId}", context.Message.Id);
  }

  private async Task CheckContentAsync(ulong guildId, ulong uploaderId, string messageId, string messageContent)
  {
    var splitMessageContent = messageContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    foreach (var word in splitMessageContent)
    {
      try
      {
        var url = RegexHelper.Url(word);
        if (string.IsNullOrEmpty(url))
          continue;

        if (IsFromStaticUrl(url))
        {
          _logger.LogDebug("URL {Url} is from static source. Skipping.", url);
          continue;
        }

        _logger.LogDebug("Processing URL {Url} from message {MessageId}", url, messageId);
        var attachment = await GetFromUriAsync(url);
        attachment.Name = $"{messageId}{Path.GetExtension(url)}";

        await SendToApiAsync(guildId, uploaderId, attachment);
        _logger.LogInformation("Attachment from URL {Url} sent to API.", url);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "[CheckContentAsync] Failed to process word '{Word}' in message {MessageId}", word, messageId);
      }
    }
  }

  private async Task CheckAttachmentsAsync(ulong guildId, ulong uploaderId, IReadOnlyCollection<Discord.Attachment> attachments)
  {
    foreach (var file in attachments)
    {
      try
      {
        _logger.LogDebug("Processing attachment {Filename} ({Url})", file.Filename, file.Url);
        var attachment = await GetFromUriAsync(file.Url);
        attachment.Name = $"{file.Id}{Path.GetExtension(file.Filename)}";

        await SendToApiAsync(guildId, uploaderId, attachment);
        _logger.LogInformation("Attachment {Filename} sent to API.", file.Filename);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "[CheckAttachmentsAsync] Failed to process attachment '{Filename}'", file.Filename);
      }
    }
  }

  private async Task<Attachment> GetFromUriAsync(string uri)
  {
    if (string.IsNullOrWhiteSpace(uri))
    {
      _logger.LogWarning("GetFromUriAsync called with null or empty uri.");
      throw new ArgumentNullException(nameof(uri), "Uri string was null or empty.");
    }

    using var client = new HttpClient();
    using var response = await client.GetAsync(uri);
    response.EnsureSuccessStatusCode();

    var contentType = response.Content.Headers.ContentType?.MediaType ?? throw new Exception("Content type is missing.");
    var ms = new MemoryStream();
    await response.Content.CopyToAsync(ms);
    ms.Position = 0; // Reset position for reading elsewhere

    var attachment = new Attachment
    {
      MimeType = contentType,
      Data = ms
    };

    _logger.LogDebug("Getting the main type of the content type: {contenttype}", contentType);

    var mainType = contentType.Split('/')[0];
    if (mainType != "image" && mainType != "video" && mainType != "audio")
    {
      _logger.LogWarning("Unsupported content type {ContentType} for uri {Uri}", attachment.MimeType, uri);
      throw new Exception($"The content type {attachment.MimeType} of the URL is not supported.");
    }

    _logger.LogDebug("Successfully downloaded attachment from {Uri} with content type {ContentType}", uri, contentType);

    return attachment;
  }

  private async Task SendToApiAsync(ulong guildId, ulong uploaderId, Attachment attachment)
  {
    var mainType = attachment.MimeType.Split('/')[0];
    _logger.LogDebug("Sending attachment of type {MainType} to API. Guild: {GuildId}, Uploader: {UploaderId}, Name: {Name}", mainType, guildId, uploaderId, attachment.Name);

    switch (mainType)
    {
      case "image":
        _logger.LogInformation("Image attachment {Name} sent to API.", attachment.Name);
        await _imageService.PostAsync(guildId, uploaderId, attachment);
        break;
      case "video":
        _logger.LogInformation("Video attachment {Name} sent to API.", attachment.Name);
        await _videoService.PostAsync(guildId, uploaderId, attachment);
        break;
      case "audio":
        _logger.LogInformation("Audio attachment {Name} sent to API.", attachment.Name);
        await _audioService.PostAsync(guildId, uploaderId, attachment);
        break;
      default:
        _logger.LogWarning("Attempted to send unsupported attachment type: {MimeType}", attachment.MimeType);
        throw new Exception($"Unsupported attachment type: {attachment.MimeType}");
    }
  }

  public bool IsFromStaticUrl(string url)
  {
    try
    {
      var staticUri = new Uri(_apiSettings.StaticUrl);
      var urlUri = new Uri(url);

      // Check if the static domain is a suffix of the incoming url's host (allowing subdomains)
      var isStatic = urlUri.Host.EndsWith(staticUri.Host, StringComparison.OrdinalIgnoreCase);
      if (isStatic)
        _logger.LogDebug("URL {Url} is recognized as static content.", url);

      return isStatic;
    }
    catch
    {
      // Fallback: just check if staticUrl is a substring of the incoming url
      var isStatic = url.Contains(_apiSettings.StaticUrl, StringComparison.OrdinalIgnoreCase);
      if (isStatic)
        _logger.LogDebug("URL {Url} matched static URL as substring.", url);
      return isStatic;
    }
  }
}