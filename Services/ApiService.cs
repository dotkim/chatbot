using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;
using ChatBot.Security;

namespace ChatBot.Services
{
  public class ApiService
  {
    private readonly HttpClient _http;
    private readonly Configuration _config;

    public ApiService(HttpClient http)
    {
      _http = http;
      _config = new ConfigurationLoader().LoadConfig();
    }

    public async Task<NamedStream> GetRandomImage(string guildId, string tag = null)
    {
      string url = _config.ApiUrl + _config.RandomService.Replace("%1", guildId);
      if (!string.IsNullOrEmpty(tag)) url += $"&tag={tag}";

      var apiResp = await _http.GetAsync(url);
      string apiRespContent = await apiResp.Content.ReadAsStringAsync();
      var imageInfo = Json.Deserialize<ApiImage>(apiRespContent);

      var resp = await _http.GetAsync(imageInfo.Url);

      return new NamedStream { Info = imageInfo, ImageStream = await resp.Content.ReadAsStreamAsync() };
    }

    public async Task ExcludeImageFromGuild(string name, string guildId)
    {
      var exclude = new ExcludeImageContent { Name = name, GuildId = guildId };
      string stringyfiedJson = Json.Serialize(exclude);

      var content = new StringContent(stringyfiedJson, Encoding.UTF8, "application/json");

      var requestMessage = new HttpRequestMessage(HttpMethod.Post, _config.ApiUrl + _config.ExcludeService);
      requestMessage.Headers.Authorization = Authentication.GetAuthenticationString();
      requestMessage.Content = content;

      var resp = await _http.SendAsync(requestMessage);
      resp.EnsureSuccessStatusCode();
    }
  }
}
