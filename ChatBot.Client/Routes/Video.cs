using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/video/random/{GuildId}", "GET")]
  [Route("/video/random/{GuildId}/{Filter}", "GET")]
  public class GetVideoRandom : IReturn<GetVideoRandomResponse>
  {
    public ulong GuildId { get; set; }
    public string Filter { get; set; } = "tagme";
  }

  [Route("/video/{GuildId}", "POST")]
  public class PostVideo
  {
    public ulong GuildId { get; set; }
  }

  public class GetVideoRandomResponse
  {
    public Video FileInfo { get; set; }
  }
}
