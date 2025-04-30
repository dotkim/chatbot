using System;

namespace Chatbot.Client.Types;

public class Audio
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public long GuildId { get; set; }
  public long UploaderId { get; set; }
  public string? Extension { get; set; }
  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
  public DateTimeOffset ModifiedOn { get; set; }
}