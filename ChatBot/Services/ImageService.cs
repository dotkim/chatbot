using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Libraries;
using ChatBot.Types;

namespace ChatBot.Services;

public static class ImageService
{
  private static readonly Configuration _Config = new ConfigurationLoader().LoadConfig<Configuration>();
  private static readonly ApiClient _Api = new(_Config.BaseUrl, _Config.Username, _Config.Password);

  private static List<string> Previous = [];

  public async static Task<string> GetRandomAsync(ulong guild, string filter = "tagme", int depth = 0)
  {
    if (depth >= 10)
    {
      Previous.Clear();
      throw new TaskCanceledException("The GetRandom method looped to many times. Lower the AvoidDupeCount setting to match the amount of files.");
    }

    string route = $"image/{guild}/{filter}";
    string response = await _Api.GetAsync(route);

    Image image = JsonSerializer.Deserialize<Image>(response);

    if (Previous.Contains(image.Name))
    {
      return await GetRandomAsync(guild, filter, depth + 1);
    }
    else
    {
      if (Previous.Count >= _Config.AvoidDupeCount) Previous.RemoveAt(0);
      Previous.Add(image.Name);
      string url = Path.Combine(_Config.StaticUrl, image.Name);
      return url;
    }
  }

  public static void Post(ulong guild, ulong uploader, Attachment attachment)
  {
    string path = "image/" + guild + "/" + uploader;
    var response = _Api.PostFileAsync(path, attachment.Data, attachment.Name, attachment.MimeType);
  }
}
