using System.Collections.Generic;
using System.Text.Json;

namespace ChatBot.Libraries
{
  public static class Json
  {
    public static T Deserialize<T>(string json) where T : class
    {
      return JsonSerializer.Deserialize<T>(json);
    }

    public static string Serialize(object keyValuePairs)
    {
      return JsonSerializer.Serialize(keyValuePairs);
    }
  }
}