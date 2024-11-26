using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("Properties\\launchSettings.json")
    .AddEnvironmentVariables()
    .Build();