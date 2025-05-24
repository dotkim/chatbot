using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ChatBot;

public class Program
{
  public static async Task Main(string[] args)
  {
    var host = Host.CreateDefaultBuilder(args)
      .ConfigureServices((context, services) =>
      {
        var startup = new Startup(context.Configuration);
        startup.ConfigureServices(context, services);
      })
      .ConfigureLogging((context, logging) =>
      {
        var startup = new Startup(context.Configuration);
        startup.ConfigureLogging(context, logging);
      })
      .Build();

    System.Console.WriteLine("running host.");
    await host.RunAsync();
  }
}