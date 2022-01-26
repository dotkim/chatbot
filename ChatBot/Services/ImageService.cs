using System.IO;
using System.Threading.Tasks;
using ChatBot.Client;
using ChatBot.Client.Routes;
using ChatBot.Types;
using ChatBot.Libraries;
using System.Collections.Generic;

namespace ChatBot.Services
{
  public static class ImageService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();
    private static ApiClient _api = new ApiClient(_config.BaseUrl, _config.Username, _config.Password);
    
    private static List<string> _Previous = new List<string>();

    public async static Task<string> GetRandomAsync(ulong guild, string filter = "tagme", int depth = 0)
    {
      if (depth >= 10)
      {
        _Previous.Clear();
        throw new TaskCanceledException("The GetRandom method looped to many times. Lower the AvoidDupeCount setting to match the amount of files.");
      }

      var query = new GetImageRandom { GuildId = guild, Filter = filter };
      GetImageRandomResponse response = await _api.client.GetAsync(query);

      if (_Previous.Contains(response.FileInfo.Name))
      {
        return await GetRandomAsync(guild, filter, depth+1);
      }
      else
      {
        if (_Previous.Count >= _config.AvoidDupeCount) _Previous.RemoveAt(0);
        _Previous.Add(response.FileInfo.Name);
        string url = Path.Combine(_config.StaticUrl, response.FileInfo.Name);
        return url;
      }
    }

    public static void Post(ulong guild, ulong uploader, Attachment attachment)
    {
      string path = "image/" + guild + "/" + uploader;
      var response = _api.client.PostFile<PostImage>(path, attachment.Data, attachment.Name, attachment.MimeType);
    }
  }
}
