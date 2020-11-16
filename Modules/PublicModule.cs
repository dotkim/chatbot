using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using ChatBot.Services;

namespace ChatBot.Modules
{
  public class PublicModule : ModuleBase<SocketCommandContext>
  {
    public ApiService ApiService { get; set; }
    public KeywordService KeywordService { get; set; }

    [Command("cry")]
    public async Task CryAsync()
    {
      var stream = await ApiService.GetCryPictureAsync();
      stream.Seek(0, SeekOrigin.Begin);
      await Context.Channel.SendFileAsync(stream, "cat.jpg");
    }

    [Command("keyword")]
    [Alias("kw")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The keyword command only works from a guild.")]
    public async Task KeywordAsync(string keyword)
    {
      //string keyword = Context.Message.Content.Split(" ")[1];
      string messageToSend = await KeywordService.GetKeywordAsync(Context.Guild.Id, keyword);
      await Context.Channel.SendMessageAsync(messageToSend);
    }
  }
}
