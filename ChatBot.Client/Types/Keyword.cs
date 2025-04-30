using System;
using System.Collections.Generic;

namespace Chatbot.Client.Types;

public class Keyword
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public long GuildId { get; set; }
  public long UploaderId { get; set; }
  public List<Message> Messages { get; } = [];
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
  public DateTimeOffset ModifiedOn { get; set; }
}
