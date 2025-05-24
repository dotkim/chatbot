using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Types;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class VideoService
{
  private readonly ApiSettings _config;
  private readonly ApiClient _api;
  private readonly int _avoidDupeCount;
  private readonly List<string> _previous = [];

  public VideoService(
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

    var video = await FetchVideoAsync($"video/{guild}");

    if (_previous.Contains(video.Name))
    {
      return await GetRandomAsync(guild, depth + 1);
    }

    if (_previous.Count >= _avoidDupeCount)
      _previous.RemoveAt(0);

    _previous.Add(video.Name);

    return BuildVideoUrl(video.Name);
  }

  public async Task<string> GetByTagAsync(ulong guild, string tag)
  {
    var video = await FetchVideoAsync($"video/{guild}/{tag}");
    return BuildVideoUrl(video.Name);
  }

  public async Task<Video> GetByNameAsync(ulong guild, string name)
  {
    var video = await FetchVideoAsync($"video/single/{guild}/{name}");
    return video;
  }

  public async Task<string> TagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("video/tag", body);
    return response;
  }

  public async Task<string> UntagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("video/untag", body);
    return response;
  }

  public async Task<string> DeleteAsync(ulong guild, string name)
  {
    var response = await _api.DeleteFileAsync($"video/{guild}/{name}");
    return response;
  }

  private async Task<Video> FetchVideoAsync(string route)
  {
    string response = await _api.GetAsync(route);
    return JsonSerializer.Deserialize<Video>(response);
  }

  private string BuildVideoUrl(string videoName)
  {
    return Path.Combine(_config.StaticUrl, "videos", videoName);
  }

  public async Task PostAsync(ulong guild, ulong uploader, Attachment attachment)
  {
    string path = $"video/{guild}/{uploader}";
    await _api.PostFileAsync(path, attachment.Data, attachment.Name, attachment.MimeType);
  }
}