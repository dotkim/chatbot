using System.Threading.Tasks;
using ChatBot.Services;
using Discord.Commands;
using Discord.Interactions;

namespace ChatBot.Modules
{
  public class PublicModule : ModuleBase<SocketCommandContext>
  {
    [Command("cry", true)]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The cry command only works from a guild.")]
    [SlashCommand("cry", "Send a random crying cat.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task CryAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id, "cry");
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("brainlet", true)]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The brainlet command only works from a guild.")]
    [SlashCommand("brainlet", "Send a random brainlet.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task BrainletAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id, "brainlet");
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("keyword")]
    [Alias("k")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The keyword command only works from a guild.")]
    [SlashCommand("keyword", "Send a keyword message.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task KeywordAsync(string keyword)
    {
      string messageToSend = await KeywordService.GetAsync(keyword, Context.Guild.Id);
      await Context.Channel.SendMessageAsync(messageToSend);
    }

    [Command("help", true)]
    [Alias("h")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The help command only works from a guild.")]
    [SlashCommand("help", "Show the help dialog.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task Help()
    {
      string commands = "Available commands: random(r), vandom(v), aandom(a), cry, keyword(k) [name], add, help(h)\n";
      string kinfo = "How to add a keyword:\n!add name text e.g. !add glenn Nå må jeg sove\n";
      var keywords = await KeywordService.GetAllNames(Context.Guild.Id);
      string formatted = "Here is a list of keywords:\n" + string.Join("\n", keywords);
      await Context.Channel.SendMessageAsync(commands + kinfo + formatted);
    }

    [Command("add")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The add command only works from a guild.")]
    [SlashCommand("add", "Add a keyword.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task Add(string name, [Remainder] string message)
    {
      const int delay = 3000;
      KeywordService.Post(name, Context.Guild.Id, Context.Message.Author.Id, message);
      var res = await Context.Channel.SendMessageAsync("Added keyword.");
      await Task.Delay(delay);
      await res.DeleteAsync();
      await Context.Message.DeleteAsync();
    }

    [Command("random", true)]
    [Alias("r")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The random command only works from a guild.")]
    [SlashCommand("random", "Send a random image file.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task GetRandomImageAsync()
    {
      var image = await ImageService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("vandom", true)]
    [Alias("v")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The vandom command only works from a guild.")]
    [SlashCommand("vandom", "Send a random video file.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task GetRandomVideoAsync()
    {
      var video = await VideoService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(video);
    }

    [Command("aandom", true)]
    [Alias("a")]
    [Discord.Commands.RequireContext(Discord.Commands.ContextType.Guild, ErrorMessage = "The aandom command only works from a guild.")]
    [SlashCommand("aandom", "Send a random audio file.")]
    [Discord.Interactions.RequireContext(Discord.Interactions.ContextType.Guild)]
    public async Task GetRandomAudioAsync()
    {
      var audio = await AudioService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(audio);
    }
  }
}
