using System.Collections.Generic;
using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/keyword/{GuildId}", Verbs = "GET")]
  public class GetKeywordNames : IReturn<GetKeywordNamesResponse>
  {
    public ulong GuildId { get; set; }
  }

  [Route("/keyword/{Name}/{GuildId}", "GET")]
  public class GetKeyword : IReturn<GetKeywordResponse>
  {
    public string Name { get; set; }
    public ulong GuildId { get; set; }
  }

  public class GetKeywordResponse
  {
    public Message Result { get; set; }
  }

  public class GetKeywordNamesResponse
  {
    public List<string> Result { get; set; }
  }

  [Route("/keyword", "POST")]
  public class PostKeyword : IReturn<GetKeywordResponse>
  {
    public string Name { get; set; }
    public ulong GuildId { get; set; }
    public ulong UploaderId { get; set; }
    public string Message { get; set; }
  }
}
