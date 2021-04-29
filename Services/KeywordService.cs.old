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
      long lId = (long) id;

      Keyword result = await db.LoadKeywordAsync(lId, keyword);
      await db.TriggerKeywordUseCount(result);
      return result.Message;
    }

    public async Task<string> AddKeywordAsync(ulong id, string name, string message)
    {
      var db = new Database();
      long lId = (long) id;

      int lowestUseCount;
      try
      {
        lowestUseCount = (await db.LoadKeywordAsync(lId, name)).UseCount;
      }
      catch
      {
        lowestUseCount = 0;
      }

      Keyword kw = new Keyword
      {
        Name = name,
        GuildId = lId,
        Message = message,
        MessageType = "text",
        UseCount = lowestUseCount
      };

      int result = await db.InsertKeywordAsync(kw);
      return kw.Message;
    }
  }
}