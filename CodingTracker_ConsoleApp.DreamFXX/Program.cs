using Microsoft.Extensions.Configuration;
using Spectre.Console;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("Properties\\launchSettings.json")
    .AddEnvironmentVariables()
    .Build();

string connectionString = config.GetValue<string>("connectionString") ?? "";

if (string.IsNullOrEmpty(connectionString))
{
    AnsiConsole.MarkupLine("[bold][red]One of the following problems has occured![/][/]\n\t 1. Your connectionString is null.\n\t 2. There is some error with your database (file path to db file, wrong connectionString etc.)\n\n [underline]HINT - Look into my repository on my GitHub to view guide for setup of this project.");
    return;
}

DatabaseManager databaseManager = new DatabaseManager(connectionString);
AppManager appManager = new AppManager(databaseManager);
try
{
    appManager.AppStart();
}
catch (Exception e)
{

    throw new Exception(e.Message);
}
