using System.IO;
using ChatBot.Libraries;

namespace ChatBot.Common
{
  public static class InitializeApplication
  {
    public static void Init()
    {
      SetupDatabase();
    }

    private static void SetupDatabase()
    {
      if (File.Exists("./ChatBot.sqlite")) return;
      new Database().CreateDatabaseAndTables();
    }
  }
}