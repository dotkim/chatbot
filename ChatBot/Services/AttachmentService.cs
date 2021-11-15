using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;
using Discord.Commands;

namespace ChatBot.Services
{
  public static class AttachmentService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();
    private static readonly HttpClient _http = new HttpClient();

    public static void CheckAndFetchAttachment(SocketCommandContext context)
    {
      if (context.Message.Attachments.Count > 0)
        CheckAttachments(context.Guild.Id, context.Message.Attachments);

      if (context.Message.Content.Length > 0)
        CheckContent(context.Guild.Id, context.Message.Id.ToString(), context.Message.Content);
    }

    private static async void CheckContent(ulong guild, string id, string messageContent)
    {
      string[] splitMessageContent = messageContent.Split(" ");

      foreach (string str in splitMessageContent)
      {
        try
        {
          string url = RegexHelper.Url(str);

          string staticUrl = _config.StaticUrl.Split(".").Length > 1
            ? _config.StaticUrl.Split(".")[1] : "localhost";

          if (url.Contains(staticUrl)) continue; // Do not upload an image the api already has.
          if (string.IsNullOrEmpty(url)) continue;                     // The url variable will return a blank string if no url was found.

          // GET request for the attachment Url.
          Attachment attachment = await GetFromUri(url);

          string extension =
            new Stack<string>(
              url.Split(".")
            ).Pop();

          attachment.Name = id + "." + extension;

          SendToApi(guild, attachment);
        }
        catch (Exception err)
        {
          Console.WriteLine(err.ToString());
        }
      }
    }

    private static async void CheckAttachments(ulong guild, IReadOnlyCollection<Discord.Attachment> attachments)
    {
      foreach (var file in attachments)
      {
        try
        {
          // GET request for the attachment Url.
          Attachment attachment = await GetFromUri(file.Url);

          string extension =
            new Stack<string>(
              file.Filename.Split(".")
            ).Pop();

          attachment.Name = file.Id + "." + extension;

          SendToApi(guild, attachment);
        }
        catch (Exception err)
        {
          Console.WriteLine(err.ToString());
        }
      }
    }

    private static async Task<Attachment> GetFromUri(string uri)
    {
      if (string.IsNullOrEmpty(uri)) throw new NullReferenceException("Uri string was null or empty.");
      var response = await _http.GetAsync(uri);
      response.EnsureSuccessStatusCode();


      var attachment = new Attachment();
      attachment.MimeType = response.Content.Headers.ContentType.MediaType;

      switch (attachment.MimeType.Split("/")[0])
      {
        case "image":
          break;
        case "video":
          break;
        case "audio":
          break;
        default:
          throw new Exception($"The content type ${attachment.MimeType} of the URL is not supported.");
      }

      attachment.Data = await response.Content.ReadAsStreamAsync();

      return attachment;
    }

    private static void SendToApi(ulong guild, Attachment attachment)
    {
      switch (attachment.MimeType.Split("/")[0])
      {
        case "image":
          ImageService.Post(guild, attachment);
          break;
        case "video":
          VideoService.Post(guild, attachment);
          break;
        case "audio":
          AudioService.Post(guild, attachment);
          break;
      }
    }
  }
}
