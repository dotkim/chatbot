using System.IO;

namespace ChatBot.Types
{
  public class NamedStream
  {
    public ApiImage Info { get; set; }
    public Stream ImageStream { get; set; }
  }
}