using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ChatBot.Libraries;
using ChatBot.Types;

namespace ChatBot.Services
{
  public class PictureService
  {
    private readonly HttpClient _http;

    public PictureService(HttpClient http)
        => _http = http;

    public async Task<Stream> GetCryPictureAsync()
    {
      var loader = new ConfigurationLoader();
      Configuration config = loader.LoadConfig();
      
      var resp = await _http.GetAsync(config.CryUrl);
      return await resp.Content.ReadAsStreamAsync();
    }
  }
}
