using System.Text.RegularExpressions;

namespace ChatBot.Libraries
{
  public static class RegexHelper
  {
    /// <summary>
    /// Check if a string is an URL.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Url(string input)
    {
      string pattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
      var rx = new Regex(pattern);
      Match result = rx.Match(input);
      
      return result.Value;
    }
  }
}
