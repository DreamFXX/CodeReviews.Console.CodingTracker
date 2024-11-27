using CodingTracker_ConsoleApp.DreamFXX;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("Properties\\launchSettings.json")
    .AddEnvironmentVariables()
    .Build();

string connectionString = config.GetValue<string>("connectionString") ?? "";

if (string.IsNullOrEmpty(connectionString))
{

}

DatabaseManager databaseManager = new DatabaseManager(connectionString);
AppManager appManager = new AppManager(databaseManager);
try
{
    appManager.AppStart();
}
catch (Exception e)
{

    throw new Exception(e);
}

Console.ReadKey();