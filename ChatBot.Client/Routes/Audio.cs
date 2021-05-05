using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/audio/random/{GuildId}", "GET")]
  public class GetAudioRandom : IReturn<GetAudioRandomResponse>
  {
    public ulong GuildId { get; set; }
  }

  [Route("/audio/{GuildId}", "POST")]
  public class PostAudio
  {
    public ulong GuildId { get; set; }
  }

  public class GetAudioRandomResponse
  {
    public Audio Result { get; set; }
  }
}
