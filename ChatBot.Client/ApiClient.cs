using ServiceStack;

namespace ChatBot.Client
{
  public class ApiClient
  {
    public IServiceClient client;

    public ApiClient(string url, string username, string password)
    {
      client = new JsonServiceClient(url)
      {
        UserName = username,
        Password = password,
        AlwaysSendBasicAuthHeader = true
      };
    }
  }
}
