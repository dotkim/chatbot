using ChatBot.Types;
using SQLite;
using System.Threading.Tasks;

namespace ChatBot.Libraries
{
  public class Database
  {
    private SQLiteAsyncConnection Db { get; set; }

    public Database()
    {
      var options = new SQLiteConnectionString(":memory:");
      Db = new SQLiteAsyncConnection(options);
    }

    public async void CreateDatabaseAndTables()
    {
      await Db.CreateTableAsync<Keyword>();
    }

    public async Task<Keyword> LoadKeywordAsync(ulong GuildId, string kw)
    {
      var query = Db.Table<Keyword>()
      .Where(k => k.GuildId.Equals(GuildId) & k.Name.Equals(kw))
      .OrderBy(k => k.UseCount);

      return await query.FirstAsync();
    }

    public async Task<int> InsertKeywordAsync(Keyword keyword)
    {
      return await Db.InsertAsync(keyword);
    }
  }
}