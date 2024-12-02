using Spectre.Console;

namespace MyCodingTrack
{
    public static class ProgramController
    {
        private static readonly DatabaseManager DbManager = new();
        private static readonly UserInput Input = new();

        private static void MainMenu()
        {
            var selectRoute = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Welcome in Coding Time Tracker.\n\n[underline][yellow]MAIN MENU[/][/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "View all tracked sessions",
                    "Start a new session record",
                    "Change an existing session data",
                    "Delete coding session",
                    "Close application",
                }));
            StartProgram(selectRoute);
        }

        public static void StartProgram(string selection)
        {
            DbManager.CreateDatabase();

            MainMenu();

            while (selection != string "Close Application")
            {
                

            }
        }
    }
}

