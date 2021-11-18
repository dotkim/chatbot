using System;
using System.Collections.Generic;

namespace ChatBot.Client.Types
{
  public class Image
  {
    public string Name { get; set; }
    public ulong GuildId { get; set; }
    public string Extension { get; set; }
    public List<string> Tags { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
  }
}
