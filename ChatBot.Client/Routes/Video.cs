using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/video/random/{GuildId}", "GET")]
  public class GetVideoRandom : IReturn<GetVideoRandomResponse>
  {
    public ulong GuildId { get; set; }
  }

  [Route("/video/{GuildId}", "POST")]
  public class PostVideo
  {
    public ulong GuildId { get; set; }
  }

  public class GetVideoRandomResponse
  {
    public Video Result { get; set; }
  }
}
