using System.Threading.Tasks;
using ChatBot.Client;
using ChatBot.Client.Routes;
using ChatBot.Types;
using ChatBot.Libraries;

namespace ChatBot.Services
{
  public static class KeywordService
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig<Configuration>();
    private static ApiClient _api = new ApiClient(_config.BaseUrl, _config.Username, _config.Password);

    public async static Task<string> GetAsync(string name, ulong guild)
    {
      var query = new GetKeyword { Name = name, GuildId = guild };
      GetKeywordResponse response = await _api.client.GetAsync(query);

      return response.Result.Message;
    }

    public static void Post(string name, ulong guild, string message)
    {
      var query = new PostKeyword
      {
        Name = name,
        GuildId = guild,
        Message = message
      };

      _api.client.Post<PostKeyword>(query);
    }
  }
}