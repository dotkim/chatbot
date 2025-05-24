using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Chatbot.Client.Types;
using ChatBot.Client;
using ChatBot.Types;
using Microsoft.Extensions.Options;

namespace ChatBot.Features;

public class ImageService
{
  private readonly ApiSettings _config;
  private readonly ApiClient _api;
  private readonly int _avoidDupeCount;
  private readonly List<string> _previous = [];

  public ImageService(
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

    var image = await FetchImageAsync($"image/{guild}");

    if (_previous.Contains(image.Name))
    {
      return await GetRandomAsync(guild, depth + 1);
    }

    if (_previous.Count >= _avoidDupeCount)
      _previous.RemoveAt(0);

    _previous.Add(image.Name);

    return BuildImageUrl(image.Name);
  }

  public async Task<string> GetByTagAsync(ulong guild, string tag)
  {
    var image = await FetchImageAsync($"image/{guild}/{tag}");
    return BuildImageUrl(image.Name);
  }

  public async Task<Image> GetByNameAsync(ulong guild, string name)
  {
    var image = await FetchImageAsync($"image/single/{guild}/{name}");
    return image;
  }

  public async Task<string> TagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("image/tag", body);
    return response;
  }

  public async Task<string> UntagAsync(MediaTagUpdateDto dto)
  {
    string body = JsonSerializer.Serialize(dto);
    var response = await _api.PutAsync("image/untag", body);
    return response;
  }

  public async Task<string> DeleteAsync(ulong guild, string name)
  {
    var response = await _api.DeleteFileAsync($"image/{guild}/{name}");
    return response;
  }

  private async Task<Image> FetchImageAsync(string route)
  {
    string response = await _api.GetAsync(route);
    return JsonSerializer.Deserialize<Image>(response);
  }

  private string BuildImageUrl(string imageName)
  {
    return Path.Combine(_config.StaticUrl, "images", imageName);
  }

  public async Task PostAsync(ulong guild, ulong uploader, Attachment attachment)
  {
    string path = $"image/{guild}/{uploader}";
    await _api.PostFileAsync(path, attachment.Data, attachment.Name, attachment.MimeType);
  }
}