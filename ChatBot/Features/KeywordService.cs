using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Types;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class KeywordService
{
  private readonly ApiClient _apiClient;

  public KeywordService(ApiClient apiClient)
  {
    _apiClient = apiClient;
  }

  public async Task<string> GetAsync(string name, ulong guild)
  {
    string route = $"keyword/{name}/{guild}";
    Message response = await _apiClient.GetKeywordMessageAsync(route);

    return response.Text;
  }

  public async Task<List<string>> GetAllNamesAsync(ulong guild)
  {
    string route = $"keyword/all/{guild}";
    string response = await _apiClient.GetAsync(route);

    List<Keyword> keywords = JsonSerializer.Deserialize<List<Keyword>>(response);
    List<string> keywordNames = [.. keywords.Select(k => k.Name)];

    return keywordNames;
  }

  public async Task PostAsync(string name, ulong guild, ulong uploader, string message)
  {
    await _apiClient.PostKeywordAsync(name, (long)guild, (long)uploader, message);
  }
}