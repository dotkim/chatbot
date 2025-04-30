using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chatbot.Client.Types;

public class Keyword
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("guildId")]
  public long GuildId { get; set; }

  [JsonPropertyName("uploaderId")]
  public long UploaderId { get; set; }

  [JsonPropertyName("messages")]
  public List<Message> Messages { get; } = [];

  [JsonPropertyName("createdOn")]
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

  [JsonPropertyName("modifiedOn")]
  public DateTimeOffset ModifiedOn { get; set; }
}
