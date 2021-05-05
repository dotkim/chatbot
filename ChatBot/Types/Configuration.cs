namespace ChatBot.Types
{
  public class Configuration
  {
    // Discord token.
    public string Token { get; set; }

    // API config.
    public string BaseUrl { get; set; }
    public string Username { get; set; }
    public string Secret { get; set; }

    // Static web URL
    public string StaticUrl { get; set; }
  }
}
