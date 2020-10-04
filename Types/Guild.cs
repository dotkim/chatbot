using SQLite;

namespace ChatBot.Types
{
  [Table("Guild")]
  public class Guild
  {
    [PrimaryKey, AutoIncrement]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Code")]
    public int Code { get;set; }
  }
}