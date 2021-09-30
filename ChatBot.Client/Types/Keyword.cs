using System;
using System.Collections.Generic;

namespace ChatBot.Client.Types
{
  public class Keyword
  {
    public string Name { get; set; }
    public ulong GuildId { get; set; }
    public List<KeywordMessage> Messages { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
  }
}
