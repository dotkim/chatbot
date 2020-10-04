using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatBot.Services
{
  public class PictureService
  {
    private readonly HttpClient _http;

    public PictureService(HttpClient http)
        => _http = http;

    public async Task<Stream> GetCryPictureAsync()
    {
      var resp = await _http.GetAsync("https://cataas.com/cat");
      return await resp.Content.ReadAsStreamAsync();
    }
  }
}
