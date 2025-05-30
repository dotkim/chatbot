using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;

namespace ChatBot.Client;

public class ApiClient
{
  private readonly HttpClient _client;

  public ApiClient(HttpClient client)
  {
    _client = client;
  }

  public async Task<string> GetAsync(string route)
  {
    using HttpResponseMessage response = await _client.GetAsync(route);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return jsonResponse;
  }

  public async Task<Message> GetKeywordMessageAsync(string route)
  {
    using HttpResponseMessage response = await _client.GetAsync(route);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadAsStringAsync();

    return JsonSerializer.Deserialize<Message>(jsonResponse);
  }

  public async Task<string> PutAsync(string route, string body)
  {
    var content = new StringContent(body, Encoding.UTF8, "application/json");
    using HttpResponseMessage response = await _client.PutAsync(route, content);
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadAsStringAsync();
  }

  public async Task PostKeywordAsync(string name, long guildId, long uploaderId, string text)
  {
    Keyword keyword = new() { Name = name, GuildId = guildId, UploaderId = uploaderId };

    using HttpResponseMessage keywordwResponse = await _client.PostAsJsonAsync("keyword", keyword);
    keywordwResponse.EnsureSuccessStatusCode();

    keyword = JsonSerializer.Deserialize<Keyword>(await keywordwResponse.Content.ReadAsStringAsync());

    string route = $"message/{name}/{guildId}";
    Message message = new() { KeywordId = keyword.Id, UploaderId = uploaderId, Text = text };

    using HttpResponseMessage messageResponse = await _client.PostAsJsonAsync(route, message);
    messageResponse.EnsureSuccessStatusCode();
  }

  public async Task<string> PostFileAsync(string route, Stream data, string filename, string mimetype)
  {
    MultipartFormDataContent content = new()
    {
      { new StreamContent(data), filename, filename }
    };

    using HttpResponseMessage response = await _client.PostAsync(route, content);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadAsStringAsync();
    return jsonResponse;
  }

  public async Task<string> DeleteFileAsync(string route)
  {
    using HttpResponseMessage response = await _client.DeleteAsync(route);
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadAsStringAsync();
  }
}
