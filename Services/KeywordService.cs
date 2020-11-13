using ChatBot.Libraries;
using ChatBot.Types;
using System.Threading.Tasks;

namespace ChatBot.Services
{
  public class KeywordService
  {
    public async Task<string> GetKeywordAsync(ulong id, string keyword)
    {
      var db = new Database();
      Keyword result = await db.LoadKeywordAsync(id, keyword);
      return result.Message;
    }
  }
}