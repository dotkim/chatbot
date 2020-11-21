using System.Text.Json.Serialization;

namespace ChatBot.Types
{
  public class ExcludeImageContent
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("guildId")]
    public string GuildId { get; set; }
  }
}