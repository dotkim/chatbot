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

    private HttpClient CreateClient()
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Clear();
      client.DefaultRequestHeaders.ConnectionClose = true;

      return client;
    }

    public async Task<Stream> GetCryPictureAsync()
    {
      var resp = await _http.GetAsync(_config.CryService);
      return await resp.Content.ReadAsStreamAsync();
    }

    public async Task<NamedStream> GetRandomImage()
    {
      var apiResp = await _http.GetAsync(_config.RandomService);
      string apiRespContent = await apiResp.Content.ReadAsStringAsync();
      var imageInfo = Json.Deserialize<ApiImage>(apiRespContent);

      var resp = await _http.GetAsync(imageInfo.Url);

      return new NamedStream { Info = imageInfo, ImageStream = await resp.Content.ReadAsStreamAsync() };
    }

    public async Task ExcludeImageFromGuild(string name, string guildId)
    {
      HttpClient client = CreateClient();

      var exclude = new ExcludeImageContent { Name = name, GuildId = guildId };
      string stringyfiedJson = Json.Serialize(exclude);

      var content = new StringContent(stringyfiedJson, Encoding.UTF8, "application/json");

      var requestMessage = new HttpRequestMessage(HttpMethod.Post, _config.ExcludeService);
      requestMessage.Headers.Authorization = Authentication.GetAuthenticationString();
      requestMessage.Content = content;

      var resp = await client.SendAsync(requestMessage);
      resp.EnsureSuccessStatusCode();
    }
  }
}
