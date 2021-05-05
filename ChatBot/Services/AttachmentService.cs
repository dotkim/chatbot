using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using ChatBot.Libraries;
using ChatBot.Types;

namespace ChatBot.Services
{
  public static class AttachmentService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();

    public static void CheckAndFetchAttachment(SocketCommandContext context)
    {
      if (context.Message.Attachments.Count > 0)
        CheckAttachments(context.Message.Attachments);

      if (context.Message.Content.Length > 0)
        CheckContent(context.Message.Id.ToString(), context.Message.Content);
    }

    private static async void CheckContent(string id, string messageContent)
    {
      string[] splitMessageContent = messageContent.Split(" ");

      foreach (string str in splitMessageContent)
      {
        try
        {
          string url = RegexHelper.Url(str);

          if (url.Contains(_config.StaticUrl.Split(".")[1])) continue;
          if (url.Length == 0) continue;

          // GET request for the attachment Url.
          byte[] content = await GetFromUri(url);

          string extension =
            new Stack<string>(
              url.Split(".")
            ).Pop();

          string name = id + "." + extension;

          SendToApi(name, content);
        }
        catch (Exception err)
        {
          Console.WriteLine(err.ToString());
        }

      }
    }

    private static async void CheckAttachments(IReadOnlyCollection<Discord.Attachment> attachments)
    {
      foreach (var attch in attachments)
      {
        try
        {
          // GET request for the attachment Url.
          byte[] content = await GetFromUri(attch.Url);

          string extension =
            new Stack<string>(
              attch.Filename.Split(".")
            ).Pop();

          string name = attch.Id + "." + extension;

          SendToApi(name, content);
        }
        catch (Exception err)
        {
          Console.WriteLine(err.ToString());
        }

      }
    }

    private static async Task<byte[]> GetFromUri(string uri)
    {
      var response = await _http.GetAsync(uri);
      response.EnsureSuccessStatusCode();

      if (!response.Content.Headers.ContentType.MediaType.Contains("image")) throw
        new Exception("Content of the URL was not an image.");

      return await response.Content.ReadAsByteArrayAsync();
    }

    private static async void SendToApi(string name, byte[] content)
    {
      string base64EncodedContent = Convert.ToBase64String(content);
      ApiInsertable body = new ApiInsertable();
      body.Name = name;
      body.Content = base64EncodedContent;

      string stringyfiedJson = Json.Serialize(body);

      HttpRequestMessage request =
        new HttpRequestMessage(HttpMethod.Post, $"{_config.ApiUrl}/images");

      request.Headers.Authorization = Authentication.GetAuthenticationString();
      request.Content =
        new StringContent(stringyfiedJson, System.Text.Encoding.UTF8, "application/json");

      var response = await _http.SendAsync(request);
      response.EnsureSuccessStatusCode();
    }
  }
}
