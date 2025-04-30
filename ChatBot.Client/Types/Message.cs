using System;
using System.Text.Json.Serialization;

namespace Chatbot.Client.Types;

public class Message
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("keywordId")]
  public long KeywordId { get; set; }

  [JsonPropertyName("uploaderId")]
  public long UploaderId { get; set; }
  
  [JsonPropertyName("text")]
  public string? Text { get; set; } = null!;

  // This is 0 by default so we dont need to set it when inserting a new Keyword.
  // It will only be one message and the count will naturally be 0.
  [JsonPropertyName("count")]
  public int Count { get; set; } = 0;

  [JsonPropertyName("createdOn")]
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

  [JsonPropertyName("modifiedOn")]
  public DateTimeOffset ModifiedOn { get; set; }
}
