using ServiceStack;
using ServiceStack.Text;

namespace ChatBot.Client
{
  public class ApiClient
  {
    public IServiceClient client;
    private ChatBot.Client.Types.Configuration _config;

    public ApiClient(ChatBot.Client.Types.Configuration config)
    {
      _config = config;
      client = new JsonServiceClient(_config.BaseUrl)
      {
        UserName = _config.UserName,
        Password = _config.Password,
        AlwaysSendBasicAuthHeader = true
      }.WithCache();
    }
  }
}
