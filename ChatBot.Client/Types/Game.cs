using System;

namespace Chatbot.Client.Types;

public class Game
{
  public long Id { get; set; }
  public string Slug { get; set; }
  public string Name { get; set; }
  public DateTimeOffset Released { get; set; }
  public DateTimeOffset Updated { get; set; }
}
