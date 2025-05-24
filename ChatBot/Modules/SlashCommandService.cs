using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chatbot.Client.Autocomplete;
using Chatbot.Client.Types;
using ChatBot.Features;
using Discord;
using Discord.Interactions;

namespace ChatBot.Modules
{
  public class SlashCommandService : InteractionModuleBase<SocketInteractionContext>
  {
    private readonly KeywordService _keywordService;
    private readonly ImageService _imageService;
    private readonly VideoService _videoService;
    private readonly AudioService _audioService;
    private readonly AttachmentService _attachmentService;

    public SlashCommandService(
        KeywordService keywordService,
        ImageService imageService,
        VideoService videoService,
        AudioService audioService,
        AttachmentService attachmentService)
    {
      _keywordService = keywordService;
      _imageService = imageService;
      _videoService = videoService;
      _audioService = audioService;
      _attachmentService = attachmentService;
    }

    [SlashCommand("keyword", "Get a saved keyword message")]
    public async Task KeywordAsync([Summary("name", "The keyword name")] string keyword)
    {
      string messageToSend = await _keywordService.GetAsync(keyword, Context.Guild.Id);
      await RespondAsync(messageToSend);
    }

    [SlashCommand("help", "Show help and all available keywords")]
    public async Task HelpAsync()
    {
      string commands = "Available commands: /keyword [name], /add, /help, /random, /vandom, /aandom\n";
      string kinfo = "How to add a keyword: `/add name text` e.g. `/add magequest amazing message`\n";
      var keywords = await _keywordService.GetAllNamesAsync(Context.Guild.Id);
      string formatted = "Here is a list of keywords:\n" + string.Join("\n", keywords);
      await RespondAsync(commands + kinfo + formatted, ephemeral: true);
    }

    [SlashCommand("info", "Get info about a file by type and message id")]
    public async Task InfoAsync(
      [Autocomplete(typeof(MediaTypeAutocompleteHandler))]
      [Summary("type", "The type of file: 'image', 'video', or 'audio'.")] string type,
      [Summary("id", "The id of the message with the file")] string messageId,
      [Summary("filename", "Optional filename to search for a specific file")] string filename = null
    )
    {
      await DeferAsync(ephemeral: true);

      if (string.IsNullOrEmpty(filename))
      {
        if (!ulong.TryParse(messageId, out var msgId))
        {
          await FollowupAsync("Invalid message ID!", ephemeral: true);
          return;
        }

        string error;
        (filename, error) = await TryGetStaticFilename(Context, msgId);
        if (filename == null)
        {
          await FollowupAsync(error, ephemeral: true);
          return;
        }
      }

      string formattedInfo = null;

      switch (type.ToLowerInvariant())
      {
        case "image":
          {
            var img = await _imageService.GetByNameAsync(Context.Guild.Id, filename);
            if (img != null)
              formattedInfo = FormatMediaInfo(img, "Image");
            break;
          }
        case "video":
          {
            var vid = await _videoService.GetByNameAsync(Context.Guild.Id, filename);
            if (vid != null)
              formattedInfo = FormatMediaInfo(vid, "Video");
            break;
          }
        case "audio":
          {
            var aud = await _audioService.GetByNameAsync(Context.Guild.Id, filename);
            if (aud != null)
              formattedInfo = FormatMediaInfo(aud, "Audio");
            break;
          }
        default:
          await FollowupAsync($"Unknown type '{type}'. Please use 'image', 'video', or 'audio'.", ephemeral: true);
          return;
      }

      if (formattedInfo == null)
      {
        await FollowupAsync("No info found for this file.", ephemeral: true);
        return;
      }

      await FollowupAsync(formattedInfo, ephemeral: true);
    }

    // Generic formatter for Image, Video, or Audio types
    private static string FormatMediaInfo(dynamic media, string label)
    {
      var tags = (media.Tags != null && media.Tags.Count > 0)
        ? string.Join(", ", ((IEnumerable<Tag>)media.Tags).Select(t => t.Name))
        : "None";

      return
        $"**{label} Information**\n" +
        $"**ID:** {media.Id}\n" +
        $"**Name:** {media.Name ?? "N/A"}\n" +
        $"**Guild ID:** {media.GuildId}\n" +
        $"**Uploader ID:** {media.UploaderId}\n" +
        $"**Extension:** {media.Extension ?? "N/A"}\n" +
        $"**Tags:** {tags}\n" +
        $"**Created On:** {media.CreatedOn:yyyy-MM-dd HH:mm:ss}\n" +
        $"**Modified On:** {(media.ModifiedOn == default(DateTimeOffset) ? "N/A" : media.ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss"))}";
    }

    [SlashCommand("tag", "Tag a file")]
    public async Task TagAsync(
      [Autocomplete(typeof(MediaTypeAutocompleteHandler))]
      [Summary("type", "the type of file e.g. image, video, audio")] string type,
      [Summary("id", "the id of the message with the file to tag")] string messageId,
      [Summary("tag", "the tag name to add.")] string tagName,
      [Summary("filename", "Optional filename to search for a specific file")] string filename = null
    )
    {
      await DeferAsync(ephemeral: true);

      if (string.IsNullOrEmpty(filename))
      {
        if (!ulong.TryParse(messageId, out var msgId))
        {
          await FollowupAsync("Invalid message ID!", ephemeral: true);
          return;
        }

        string error;
        (filename, error) = await TryGetStaticFilename(Context, msgId);
        if (filename == null)
        {
          await FollowupAsync(error, ephemeral: true);
          return;
        }
      }

      var dto = new MediaTagUpdateDto
      {
        GuildId = (long)Context.Guild.Id,
        Name = filename,
        TagName = tagName
      };

      string response;
      switch (type.ToLowerInvariant())
      {
        case "image": response = await _imageService.TagAsync(dto); break;
        case "video": response = await _videoService.TagAsync(dto); break;
        case "audio": response = await _audioService.TagAsync(dto); break;
        default:
          await FollowupAsync($"Unknown type '{type}'.", ephemeral: true); return;
      }
      await FollowupAsync(response, ephemeral: true);
    }

    [SlashCommand("untag", "Remove a tag from a file")]
    public async Task UntagAsync(
      [Autocomplete(typeof(MediaTypeAutocompleteHandler))]
      [Summary("type", "the type of file e.g. image, video, audio")] string type,
      [Summary("id", "the id of the message with the file to untag")] string messageId,
      [Summary("tag", "the tag name to remove.")] string tagName,
      [Summary("filename", "Optional filename to search for a specific file")] string filename = null
    )
    {
      await DeferAsync(ephemeral: true);

      if (string.IsNullOrEmpty(filename))
      {
        if (!ulong.TryParse(messageId, out var msgId))
        {
          await FollowupAsync("Invalid message ID!", ephemeral: true);
          return;
        }

        string error;
        (filename, error) = await TryGetStaticFilename(Context, msgId);
        if (filename == null)
        {
          await FollowupAsync(error, ephemeral: true);
          return;
        }
      }

      var dto = new MediaTagUpdateDto
      {
        GuildId = (long)Context.Guild.Id,
        Name = filename,
        TagName = tagName
      };

      string response;
      switch (type.ToLowerInvariant())
      {
        case "image": response = await _imageService.UntagAsync(dto); break;
        case "video": response = await _videoService.UntagAsync(dto); break;
        case "audio": response = await _audioService.UntagAsync(dto); break;
        default:
          await FollowupAsync($"Unknown type '{type}'.", ephemeral: true); return;
      }
      await FollowupAsync(response, ephemeral: true);
    }

    [SlashCommand("add", "Add a keyword")]
    public async Task AddAsync(
    [Summary("name", "The keyword name")] string name,
    [Summary("message", "The message for the keyword")] string message)
    {
      await DeferAsync(ephemeral: true);
      await _keywordService.PostAsync(name, Context.Guild.Id, Context.User.Id, message);
      await FollowupAsync("Added keyword.", ephemeral: true);
    }

    [SlashCommand("delete", "Delete a file by type and message id")]
    public async Task DeleteFileAsync(
      [Autocomplete(typeof(MediaTypeAutocompleteHandler))]
      [Summary("type", "The type of file: 'image', 'video', or 'audio'.")] string type,
      [Summary("id", "The id of the message with the file to delete")] string messageId,
      [Summary("filename", "Optional filename to search for a specific file")] string filename = null
    )
    {
      await DeferAsync(ephemeral: true);

      if (string.IsNullOrEmpty(filename))
      {
        if (!ulong.TryParse(messageId, out var msgId))
        {
          await FollowupAsync("Invalid message ID!", ephemeral: true);
          return;
        }

        string error;
        (filename, error) = await TryGetStaticFilename(Context, msgId);
        if (filename == null)
        {
          await FollowupAsync(error, ephemeral: true);
          return;
        }
      }

      string normalizedType = type.ToLowerInvariant();
      string response;

      switch (normalizedType)
      {
        case "image":
          response = await _imageService.DeleteAsync(Context.Guild.Id, filename);
          break;
        case "video":
          response = await _videoService.DeleteAsync(Context.Guild.Id, filename);
          break;
        case "audio":
          response = await _audioService.DeleteAsync(Context.Guild.Id, filename);
          break;
        default:
          await FollowupAsync($"Unknown type '{type}'. Please use 'image', 'video', or 'audio'.", ephemeral: true);
          return;
      }

      await FollowupAsync(response, ephemeral: true);
    }

    private async Task<(string filename, string error)> TryGetStaticFilename(SocketInteractionContext context, ulong messageId)
    {
      IMessage targetMessage = null;
      foreach (var channel in context.Guild.TextChannels)
      {
        try
        {
          var msg = await channel.GetMessageAsync(messageId);
          if (msg != null)
          {
            targetMessage = msg;
            break;
          }
        }
        catch { }
      }
      if (targetMessage == null)
        return (null, "Message not found in any text channel in this guild.");
      if (targetMessage.Author.Id != context.Client.CurrentUser.Id)
        return (null, "That message was not sent by the bot.");

      // Check for static URL in content
      var urls = ExtractUrls(targetMessage.Content);
      foreach (var url in urls)
      {
        if (_attachmentService.IsFromStaticUrl(url))
        {
          // Extract filename from the URL (after last '/')
          var filename = new Uri(url).Segments.Last();
          return (filename, null);
        }
      }

      return (null, "That message has no attachments or static image URLs to tag/untag.");
    }

    // Helper method to extract URLs from a string
    private static IEnumerable<string> ExtractUrls(string text)
    {
      var regex = new Regex(@"https?://[^\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
      return regex.Matches(text).Select(m => m.Value);
    }

    [SlashCommand("random", "Get a random image")]
    public async Task GetRandomImageAsync(
      [Summary("tag", "Optional tag to search for a specific image")] string tag = null)
    {
      string image;
      if (string.IsNullOrWhiteSpace(tag))
        image = await _imageService.GetRandomAsync(Context.Guild.Id);
      else
        image = await _imageService.GetByTagAsync(Context.Guild.Id, tag);

      await RespondAsync(image);
    }

    [SlashCommand("vandom", "Get a random video")]
    public async Task GetRandomVideoAsync(
      [Summary("tag", "Optional tag to search for a specific video")] string tag = null)
    {
      string video;
      if (string.IsNullOrWhiteSpace(tag))
        video = await _videoService.GetRandomAsync(Context.Guild.Id);
      else
        video = await _videoService.GetByTagAsync(Context.Guild.Id, tag);

      await RespondAsync(video);
    }

    [SlashCommand("aandom", "Get a random audio")]
    public async Task GetRandomAudioAsync(
      [Summary("tag", "Optional tag to search for a specific audio")] string tag = null)
    {
      string audio;
      if (string.IsNullOrWhiteSpace(tag))
        audio = await _audioService.GetRandomAsync(Context.Guild.Id);
      else
        audio = await _audioService.GetByTagAsync(Context.Guild.Id, tag);

      await RespondAsync(audio);
    }
  }
}