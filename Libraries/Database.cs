using ChatBot.Types;
using ChatBot.Libraries;
using SQLite;
using System.Threading.Tasks;

namespace ChatBot.Libraries
{
  public class Database
  {
    private SQLiteAsyncConnection Db { get; set; }
    private readonly Configuration _config;

    public Database()
    {
      _config = new ConfigurationLoader().LoadConfig();

      var options = new SQLiteConnectionString(_config.DatabasePath);
      Db = new SQLiteAsyncConnection(options);
    }

    public async void CreateDatabaseAndTables()
    {
      await Db.CreateTableAsync<Keyword>();
    }

    public async Task<Keyword> LoadKeywordAsync(long guildId, string name)
    {
      var query = Db.Table<Keyword>()
      .Where(k => k.GuildId.Equals(guildId) & k.Name.Equals(name))
      .OrderBy(k => k.UseCount);

      return await query.FirstAsync();
    }

    public async Task<int> TriggerKeywordUseCount(Keyword keyword) {
      keyword.UseCount += 1;
      return await Db.UpdateAsync(keyword);
    }

    public async Task<int> InsertKeywordAsync(Keyword keyword)
    {
      return await Db.InsertAsync(keyword);
    }
  }
}