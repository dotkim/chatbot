using System.Text.Json.Serialization;

namespace Chatbot.Client.Types;

public class Tag
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("name")]
  public required string Name { get; set; }
}