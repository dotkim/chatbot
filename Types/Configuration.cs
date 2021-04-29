namespace ChatBot.Types
{
  public class Configuration
  {
    // Discord token.
    public string Token { get; set; }

    // URL and paths to the api services.
    public string ApiUrl { get; set; }
    public string RandomService { get; set; }
    public string CryService { get; set; }
    public string ExcludeService { get; set; }
    public string GetKeyword { get; set; }

    // Auth for the API.
    public string ApiUsername { get; set; }
    public string ApiSecret { get; set; }
  }
}
