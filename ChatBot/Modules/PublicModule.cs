using System.Threading.Tasks;
using ChatBot.Services;
using Discord.Commands;

namespace ChatBot.Modules
{
  public class PublicModule : ModuleBase<SocketCommandContext>
  {
    [Command("cry", true)]
    [RequireContext(ContextType.Guild, ErrorMessage = "The cry command only works from a guild.")]
    public async Task CryAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id, "cry");
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("brainlet", true)]
    [RequireContext(ContextType.Guild, ErrorMessage = "The brainlet command only works from a guild.")]
    public async Task BrainletAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id, "brainlet");
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("keyword")]
    [Alias("k")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The keyword command only works from a guild.")]
    public async Task KeywordAsync(string keyword)
    {
      string messageToSend = await KeywordService.GetAsync(keyword, Context.Guild.Id);
      await Context.Channel.SendMessageAsync(messageToSend);
    }

    [Command("add")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The add command only works from a guild.")]
    public async Task Add(string name, [Remainder] string message)
    {
      const int delay = 3000;
      KeywordService.Post(name, Context.Guild.Id, Context.Message.Author.Id, message);
      await Task.Delay(delay);
      var res = await Context.Channel.SendMessageAsync("Added keyword.");
      await Task.Delay(delay);
      await res.DeleteAsync();
      await Context.Message.DeleteAsync();
    }

    [Command("random", true)]
    [Alias("r")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The random command only works from a guild.")]
    public async Task GetRandomImageAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("vandom", true)]
    [Alias("v")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The vandom command only works from a guild.")]
    public async Task GetRandomVideoAsync()
    {
      var video = await VideoService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(video);
    }

    [Command("aandom", true)]
    [Alias("a")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The aandom command only works from a guild.")]
    public async Task GetRandomAudioAsync()
    {
      var audio = await AudioService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(audio);
    }
  }
}
