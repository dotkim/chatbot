using System.Collections.Generic;
using ChatBot.Client.Types;
using ServiceStack;

namespace ChatBot.Client.Routes
{
  [Route("/image", "GET")]
  [Route("/image/{Page}/{Filter}", "GET")]
  public class GetImagePage : IReturn<GetImagePageResponse>
  {
    public int Page { get; set; }
    public bool Filter { get; set; }
  }

  [Route("/image/random/{GuildId}/{Filter}", "GET")]
  public class GetImageRandom : IReturn<GetImageRandomResponse>
  {
    public ulong GuildId { get; set; }
    public string Filter { get; set; }
  }

  [Route("/image/{GuildId}", "POST")]
  public class PostImage
  {
    public ulong GuildId { get; set; }
  }

  public class GetImagePageResponse
  {
    public List<Image> Result { get; set; }
  }

  public class GetImageRandomResponse
  {
    public Image Result { get; set; }
  }
}
