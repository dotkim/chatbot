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
      await Context.Channel.SendFileAsync(stream, "cat.png");
    }

    [Command("userinfo")]
    public async Task UserInfoAsync(IUser user = null)
    {
      user = user ?? Context.User;

      await ReplyAsync(user.ToString());
    }

    [Command("guild_only")]
    [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
    public Task GuildOnlyCommand()
        => ReplyAsync("Nothing to see here!");
  }
}
