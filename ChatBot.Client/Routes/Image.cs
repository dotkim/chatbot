using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/image/random/{GuildId}", "GET")]
  [Route("/image/random/{GuildId}/{Filter}", "GET")]
  public class GetImageRandom : IReturn<GetImageRandomResponse>
  {
    [ApiMember(IsRequired = true)]
    public ulong GuildId { get; set; }
    public string Filter { get; set; } = "tagme";
  }

  [Route("/image/{GuildId}/{UploaderId}", "POST")]
  public class PostImage
  {
    [ApiMember(IsRequired = true)]
    public ulong GuildId { get; set; }
    public ulong UploaderId { get; set; }
  }

  public class GetImageRandomResponse
  {
    public Image FileInfo { get; set; }
  }
}
