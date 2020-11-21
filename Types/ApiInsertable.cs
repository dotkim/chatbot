using System.Text.Json.Serialization;

namespace ChatBot.Types
{
  public class ApiInsertable
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("imageData")]
    public string Content { get; set; }
  }
}