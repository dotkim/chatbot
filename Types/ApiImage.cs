using System.Text.Json.Serialization;

namespace ChatBot.Types
{
  public class ApiImage
  {
    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
  }
}