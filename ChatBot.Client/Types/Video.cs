using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chatbot.Client.Types;

public class Video
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("guildId")]
  public long GuildId { get; set; }

  [JsonPropertyName("uploaderId")]
  public long UploaderId { get; set; }

  [JsonPropertyName("extension")]
  public string? Extension { get; set; }

  [JsonPropertyName("tags")]
  public List<Tag> Tags { get; set; } = [];

  [JsonPropertyName("createOn")]
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

  [JsonPropertyName("modifiedOn")]
  public DateTimeOffset ModifiedOn { get; set; }
}
