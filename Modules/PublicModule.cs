using System.IO;
using System.Threading.Tasks;
using ChatBot.Services;
using Discord.Commands;

namespace ChatBot.Modules
{
  public class PublicModule : ModuleBase<SocketCommandContext>
  {
    public ImageService ApiService { get; set; }
    public KeywordService KeywordService { get; set; }

    [Command("cry", true)]
    [RequireContext(ContextType.Guild, ErrorMessage = "The cry command only works from a guild.")]
    public async Task CryAsync()
    {
      var image = await ApiService.GetRandomImage(Context.Guild.Id.ToString(), "cry");
      var stream = image.ImageStream;
      stream.Seek(0, SeekOrigin.Begin);
      await Context.Channel.SendFileAsync(stream, image.Info.FileName);
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

    [Command("add")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The add command only works from a guild.")]
    public async Task AddAsync(string name, [Remainder] string message)
    {
      string addedMessage = await KeywordService.AddKeywordAsync(Context.Guild.Id, name, message);
      await Context.Channel.SendMessageAsync(addedMessage);
    }

    [Command("random", true)]
    [Alias("r")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The random command only works from a guild.")]
    public async Task GetRandomAsync()
    {
      // Fix to take Context.Guild.Id later.
      var image = await ApiService.GetRandomImage(Context.Guild.Id.ToString());
      var stream = image.ImageStream;
      stream.Seek(0, SeekOrigin.Begin);
      await Context.Channel.SendFileAsync(stream, image.Info.FileName);
    }

    [Command("exclude")]
    [Alias("ex")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The exclude command only works from a guild.")]
    public async Task ExcludeImageAsync(ulong id)
    {
      var messageToExclude = await Context.Channel.GetMessageAsync(id);
      if (messageToExclude.Attachments.Count > 0)
      {
        foreach (var image in messageToExclude.Attachments)
        {
          await ApiService.ExcludeImageFromGuild(image.Filename, Context.Guild.Id.ToString());
        }
        await Context.Channel.DeleteMessageAsync(id);
      }

    }
  }
}
