using System.Threading.Tasks;
using ChatBot.Features;
using Discord.Commands;

namespace ChatBot.Modules
{
  public class TextCommandModule : ModuleBase<SocketCommandContext>
  {
    private readonly KeywordService _keywordService;
    private readonly ImageService _imageService;
    private readonly VideoService _videoService;
    private readonly AudioService _audioService;

    public TextCommandModule(
        KeywordService keywordService,
        ImageService imageService,
        VideoService videoService,
        AudioService audioService)
    {
      _keywordService = keywordService;
      _imageService = imageService;
      _videoService = videoService;
      _audioService = audioService;
    }

    [Command("keyword")]
    [Alias("k")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The keyword command only works from a guild.")]
    public async Task KeywordAsync(string keyword)
    {
      string messageToSend = await _keywordService.GetAsync(keyword, Context.Guild.Id);
      await Context.Channel.SendMessageAsync(messageToSend);
    }

    [Command("help", true)]
    [Alias("h")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The help command only works from a guild.")]
    public async Task Help()
    {
      string commands = "Available commands: keyword(k) [name], add, help(h)\n";
      string kinfo = "How to add a keyword:\n!add name text e.g. !add magequest amazing message\n";
      var keywords = await _keywordService.GetAllNamesAsync(Context.Guild.Id);
      string formatted = "Here is a list of keywords:\n" + string.Join("\n", keywords);
      await Context.Channel.SendMessageAsync(commands + kinfo + formatted);
    }

    [Command("add")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The add command only works from a guild.")]
    public async Task Add(string name, [Remainder] string message)
    {
      const int delay = 3000;
      await _keywordService.PostAsync(name, Context.Guild.Id, Context.Message.Author.Id, message);
      var res = await Context.Channel.SendMessageAsync("Added keyword.");
      await Task.Delay(delay);
      await res.DeleteAsync();
      await Context.Message.DeleteAsync();
    }

    [Command("random", true)]
    [Alias("r")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The random command only works from a guild.")]
    public async Task GetRandomImageAsync([Remainder] string _ = null)
    {
      var image = await _imageService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(image);
    }

    [Command("vandom", true)]
    [Alias("v")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The vandom command only works from a guild.")]
    public async Task GetRandomVideoAsync([Remainder] string _ = null)
    {
      var video = await _videoService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(video);
    }

    [Command("aandom", true)]
    [Alias("a")]
    [RequireContext(ContextType.Guild, ErrorMessage = "The aandom command only works from a guild.")]
    public async Task GetRandomAudioAsync([Remainder] string _ = null)
    {
      var audio = await _audioService.GetRandomAsync(Context.Guild.Id);
      await Context.Channel.SendMessageAsync(audio);
    }
  }
}