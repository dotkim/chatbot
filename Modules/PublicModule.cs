using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ChatBot.Services;

namespace ChatBot.Modules
{
  public class PublicModule : ModuleBase<SocketCommandContext>
  {
    public PictureService PictureService { get; set; }

    [Command("cry")]
    public async Task CryAsync()
    {
      var stream = await PictureService.GetCryPictureAsync();
      stream.Seek(0, SeekOrigin.Begin);
      await Context.Channel.SendFileAsync(stream, "cat.jpg");
    }

    [Command("ping")]
    public Task PingAsync() => ReplyAsync("pong");
  }
}
