using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ChatBot.Client;
using ChatBot.Client.Routes;
using ChatBot.Client.Types;
using ChatBot.Libraries;

namespace ChatBot.Services
{
  public static class ImageService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();
    private static ApiClient _api = new ApiClient(_config);

    public async static Task<string> GetRandomAsync(ulong guild)
    {
      var query = new GetImageRandom { GuildId = guild };
      GetImageRandomResponse response = await _api.client.GetAsync(query);

      return response.Result.Name;
    }

    public static void Post(ulong guild, Stream fileStream, string fileName)
    {
      string ext = new Stack<string>(fileName.Split(new string [] { "." }, StringSplitOptions.None)).Pop();

      var query = new PostImage { GuildId = guild };
      var response = _api.client.PostFile<PostImage>("image/", fileStream, fileName, "image/" + ext);
    }
  }
}
