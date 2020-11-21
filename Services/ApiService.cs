using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;

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
      var exclude = new ExcludeImageContent { Name = name, GuildId = guildId };
      string stringyfiedJson = Json.Serialize(exclude);

      var content = new StringContent(stringyfiedJson, Encoding.UTF8, "application/json");

      var resp = await _http.PutAsync(_config.ApiUrl, content);
      if (resp.StatusCode.ToString() != "200")
        throw new System.Exception("Image was not excluded, check API server for request.");
    }
  }
}
