namespace Chatbot.Client.Types;

public class MediaTagUpdateDto
{
  public long GuildId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string TagName { get; set; } = string.Empty;
}