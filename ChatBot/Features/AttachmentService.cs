using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;
using Discord.Commands;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class AttachmentService
{
  private readonly ApiSettings _apiSettings;
  private readonly ImageService _imageService;
  private readonly VideoService _videoService;
  private readonly ImageService _audioService;

  public AttachmentService(
      IOptions<ApiSettings> apiSettingsOptions,
      ImageService imageService,
      VideoService videoService,
      ImageService audioService)
  {
    _apiSettings = apiSettingsOptions.Value ?? throw new ArgumentNullException(nameof(apiSettingsOptions));
    _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
    _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
  }

  public async Task CheckAttachmentAsync(SocketCommandContext context)
  {
    if (context?.Message == null)
      throw new ArgumentNullException(nameof(context));

    var tasks = new List<Task>();

    if (context.Message.Attachments.Count > 0)
      tasks.Add(CheckAttachmentsAsync(context.Guild.Id, context.Message.Author.Id, context.Message.Attachments));

    if (!string.IsNullOrWhiteSpace(context.Message.Content))
      tasks.Add(CheckContentAsync(context.Guild.Id, context.Message.Author.Id, context.Message.Id.ToString(), context.Message.Content));

    await Task.WhenAll(tasks);
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
          continue;

        var attachment = await GetFromUriAsync(url);
        attachment.Name = $"{messageId}{Path.GetExtension(url)}";

        await SendToApiAsync(guildId, uploaderId, attachment);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"[CheckContentAsync] {ex}");
      }
    }
  }

  private async Task CheckAttachmentsAsync(ulong guildId, ulong uploaderId, IReadOnlyCollection<Discord.Attachment> attachments)
  {
    foreach (var file in attachments)
    {
      try
      {
        var attachment = await GetFromUriAsync(file.Url);
        attachment.Name = $"{file.Id}{Path.GetExtension(file.Filename)}";

        await SendToApiAsync(guildId, uploaderId, attachment);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"[CheckAttachmentsAsync] {ex}");
      }
    }
  }

  private async Task<Attachment> GetFromUriAsync(string uri)
  {
    if (string.IsNullOrWhiteSpace(uri))
      throw new ArgumentNullException(nameof(uri), "Uri string was null or empty.");

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

    var mainType = contentType.Split('/')[0];
    if (mainType != "image" && mainType != "video" && mainType != "audio")
      throw new Exception($"The content type {attachment.MimeType} of the URL is not supported.");

    return attachment;
  }

  private async Task SendToApiAsync(ulong guildId, ulong uploaderId, Attachment attachment)
  {
    var mainType = attachment.MimeType.Split('/')[0];
    switch (mainType)
    {
      case "image":
        await _imageService.PostAsync(guildId, uploaderId, attachment);
        break;
      case "video":
        await _videoService.PostAsync(guildId, uploaderId, attachment);
        break;
      case "audio":
        await _audioService.PostAsync(guildId, uploaderId, attachment);
        break;
      default:
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
      return urlUri.Host.EndsWith(staticUri.Host, StringComparison.OrdinalIgnoreCase);
    }
    catch
    {
      // Fallback: just check if staticUrl is a substring of the incoming url
      return url.Contains(_apiSettings.StaticUrl, StringComparison.OrdinalIgnoreCase);
    }
  }
}