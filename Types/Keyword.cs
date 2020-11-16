using SQLite;

namespace ChatBot.Types
{
  [Table("Keyword")]
  public class Keyword
  {
    [PrimaryKey, AutoIncrement]
    [Column("Id")]
    public int Id { get; set; }

    [Indexed]
    [Column("GuildId")]
    public long GuildId { get; set; }

    [Indexed]
    [Column("Name")]
    public string Name { get; set; }

    [Column("Message")]
    public string Message { get; set; }

    [Column("MessageType")]
    public string MessageType { get; set; }

    [Column("UseCount")]
    public int UseCount { get; set; }
  }
}