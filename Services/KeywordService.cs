using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;
using ChatBot.Security;

namespace ChatBot.Services
{
  public class KeywordService
  {
    private readonly HttpClient _http;
    private readonly Configuration _config;

    public KeywordService(HttpClient http)
    {
      _http = http;
      _config = new ConfigurationLoader().LoadConfig();
    }

    public async Task<string> GetKeywordAsync(ulong id, string name)
    {
      string url = _config.ApiUrl + _config.GetKeyword
        .Replace("%1", name)
        .Replace("%2", id.ToString());

      var res = await _http.GetAsync(url);
      string resContent = await res.Content.ReadAsStringAsync();
      var keyword = Json.Deserialize<Keyword>(resContent);

      return keyword.Message;
    }

    public async Task<string> AddKeywordAsync(ulong id, string name, string message)
    {
      
    }
  }
}