using System.IO;

namespace ChatBot.Types
{
  public class Attachment
  {
    public string Name { get; set; }
    public string MimeType { get; set; }
    public Stream Data { get; set; }
  }
}