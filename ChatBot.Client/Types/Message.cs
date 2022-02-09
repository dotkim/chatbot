using System;

namespace ChatBot.Client.Types
{
  public class Message
  {
    public ulong UploaderId { get; set; }
    public string Text { get; set; }
    public int Count { get; set; } = 0;
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
  }
}
