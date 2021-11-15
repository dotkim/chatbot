using System;
using System.IO;
using ChatBot.Types;

namespace ChatBot.Libraries
{
  public static class Initialize
  {
    /// <summary>
    /// Should run at the start of Main/Program.
    /// Check for things that are neccessary for the application to run.
    /// </summary>
    public static void Start()
    {
      string fileName = Directory.GetCurrentDirectory() + @"/Configuration.xml";
      // Check if the configuration file exists, if it doesn't the bot will try to create it.
      if (!File.Exists(fileName))
      {
        Console.Write("No configuration file found, do you want to create one? (Y/n): ");
        ConsoleKeyInfo cki = Console.ReadKey();
        if (cki.Key == ConsoleKey.N)
        {
          // Close the application
          Console.WriteLine("This bot will not run without a configuration, please read the readme on the github page.");
          Environment.Exit(0);
        }

        // create config
        Configuration config = new Configuration();
        Console.Write("Input the Discord bot token: ");
        config.Token = Console.ReadLine();

        Console.Write("Input the Base API URL: ");
        config.BaseUrl = Console.ReadLine();

        Console.Write("Input the Static URL: ");
        config.StaticUrl = Console.ReadLine();

        // I know its ugly, dont want to fiddle with the config type.
        if (String.IsNullOrEmpty(config.BaseUrl) || String.IsNullOrEmpty(config.StaticUrl) || String.IsNullOrEmpty(config.Token))
        {
          throw new NullReferenceException("All the fields in the configuration file are required.");
        }
        else
        {
          Xml.Serialize<Configuration>(config, fileName);
        }
      }
    }
  }
}
