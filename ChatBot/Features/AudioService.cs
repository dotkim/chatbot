using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Types;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class AudioService
{
  private readonly ApiSettings _config;
  private readonly ApiClient _api;
  private readonly int _avoidDupeCount;
  private readonly List<string> _previous = [];

  public AudioService(
    IOptions<ApiSettings> configOptions,
    IOptions<ProcessingSettings> processingOptions,
    ApiClient apiClient)
  {
    _config = configOptions.Value;
    _api = apiClient;
    _avoidDupeCount = processingOptions.Value.AvoidDupeCount;
  }

  public async Task<string> GetRandomAsync(ulong guild, int depth = 0)
  {
    if (depth >= 10)
    {
      _previous.Clear();
      throw new TaskCanceledException("The GetRandom method looped too many times. Lower the AvoidDupeCount setting to match the amount of files.");
    }

    var audio = await FetchAudioAsync($"audio/{guild}");

    if (_previous.Contains(audio.Name))
    {
      return await GetRandomAsync(guild, depth + 1);
    }

    if (_previous.Count >= _avoidDupeCount)
      _previous.RemoveAt(0);

    _previous.Add(audio.Name);

    return BuildAudioUrl(audio.Name);
  }

  public async Task<string> GetByTagAsync(ulong guild, string tag)
  {
    var audio = await FetchAudioAsync($"audio/{guild}/{tag}");
    return BuildAudioUrl(audio.Name);
  }

  public async Task<Audio> GetByNameAsync(ulong guild, string name)
  {
    var audio = await FetchAudioAsync($"audio/single/{guild}/{name}");
    return audio;
  }

  public async Task<string> TagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("audio/tag", body);
    return response;
  }

  public async Task<string> UntagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("audio/untag", body);
    return response;
  }

  public async Task<string> DeleteAsync(ulong guild, string name)
  {
    var response = await _api.DeleteFileAsync($"audio/{guild}/{name}");
    return response;
  }

  private async Task<Audio> FetchAudioAsync(string route)
  {
    string response = await _api.GetAsync(route);
    return JsonSerializer.Deserialize<Audio>(response);
  }

  private string BuildAudioUrl(string audioName)
  {
    return Path.Combine(_config.StaticUrl, "audio", audioName);
  }

  public async Task PostAsync(ulong guild, ulong uploader, Attachment attachment)
  {
    string path = $"audio/{guild}/{uploader}";
    await _api.PostFileAsync(path, attachment.Data, attachment.Name, attachment.MimeType);
  }
}