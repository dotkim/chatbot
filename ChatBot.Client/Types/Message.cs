using System;

namespace Chatbot.Client.Types;

public class Message
{
  public long Id { get; set; }
  public long KeywordId { get; set; }
  public long UploaderId { get; set; }
  public string? Text { get; set; } = null!;

  // This is 0 by default so we dont need to set it when inserting a new Keyword.
  // It will only be one message and the count will naturally be 0.
  public int Count { get; set; } = 0;
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
  public DateTimeOffset ModifiedOn { get; set; }
}
