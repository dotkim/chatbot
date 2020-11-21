using System.Net.Http.Headers;
using ChatBot.Libraries;
using ChatBot.Types;

namespace ChatBot.Security
{
  public static class Authentication
  {
    private static readonly Configuration _config = new ConfigurationLoader().LoadConfig();

    public static AuthenticationHeaderValue GetAuthenticationString()
    {
      var authenticationString = $"{_config.ApiUsername}:{_config.ApiSecret}";
      var base64EncodedAuthenticationString =
        System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
      return new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
  }
}