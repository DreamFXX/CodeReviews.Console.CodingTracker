using Microsoft.Extensions.Configuration;

namespace ConsoleCodingTracker.DreamFXX
{
    class Program
    {
             private static IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("launchSettings.json")
            .AddEnvironmentVariables()
            .Build();

        static string? connectionString = config.GetValue<string>("connectionString");

        static void Main(string[] args)
        {
            DbController dbController = new(connectionString);
            GetUserInput getUserInput = new();
        }
    }
}
