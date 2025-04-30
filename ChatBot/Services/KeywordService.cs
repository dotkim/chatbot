using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Libraries;
using ChatBot.Types;

namespace ChatBot.Services;

public static class KeywordService
{
  private static readonly Configuration _Config = new ConfigurationLoader().LoadConfig<Configuration>();
  private static readonly ApiClient _Api = new(_Config.BaseUrl, _Config.Username, _Config.Password);

  public async static Task<string> GetAsync(string name, ulong guild)
  {
    string route = $"keyword/{name}/{guild}";
    Message response = await _Api.GetKeywordMessageAsync(route);

    return response.Text;
  }

  public async static Task<List<string>> GetAllNamesAsync(ulong guild)
  {
    string route = $"all/{guild}";
    string response = await _Api.GetAsync(route);

    List<Keyword> keywords = JsonSerializer.Deserialize<List<Keyword>>(response);
    List<string> keywordNames = [.. keywords.Select(k => k.Name)];

    return keywordNames;
  }

  public static void Post(string name, ulong guild, ulong uploader, string message)
  {
    long longGuild = (long)guild + long.MinValue;
    long longUploader = (long)uploader + long.MinValue;
    _Api.PostKeywordAsync(name, longGuild, longUploader, message);
  }
}
