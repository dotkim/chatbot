using System.IO;
using System.Threading.Tasks;
using ChatBot.Client;
using ChatBot.Client.Routes;
using ChatBot.Types;
using ChatBot.Libraries;

namespace ChatBot.Services
{
  public static class AudioService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();
    private static ApiClient _api = new ApiClient(_config.BaseUrl, _config.Username, _config.Password);

    public async static Task<string> GetRandomAsync(ulong guild, string filter = "tagme")
    {
      var query = new GetAudioRandom { GuildId = guild, Filter = filter  };
      GetAudioRandomResponse response = await _api.client.GetAsync(query);

      string url = Path.Combine(_config.StaticUrl, response.FileInfo.Name);
      return url;
    }

    public static void Post(ulong guild, Attachment attachment)
    {
      var response = _api.client.PostFile<PostAudio>("audio/" + guild, attachment.Data, attachment.Name, attachment.MimeType);
    }
  }
}