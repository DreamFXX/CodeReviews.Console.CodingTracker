using Spectre.Console;

namespace MyCodingTracker
{
    public static class ProgramController
    {
        private static readonly DatabaseManager DbManager = new();
        private static readonly UserInput Input = new();

        static void MainMenu()
        {
            
            string menuPrompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[yellow]Welcome in Coding Time Tracker![/]\n[underline][yellow]MAIN MENU[/][/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "View all tracked sessions",
                    "Start a new session record",
                    "Change an existing session data",
                    "Delete coding session",
                    "Close application",
                }));

        }

        public static void StartProgram(string menuPrompt)
        {
            DbManager.CreateDatabase();

            MainMenu();
            
            while (menuPrompt != "Close application")
            {
                switch (menuPrompt)
                {
                    case "View all tracked sessions":
                        ViewAllSessions();
                        break;
                    case "Start a new session record":
                        StartNewSession();
                        break;
                    case "Change an existing session data":
                        ChangeSessionData();
                        break;
                    case "Delete coding session":
                        DeleteSession();
                        break;
                    case "Close application":
                        Environment.Exit(0);
                        break;
                    case "":
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]This menu route is broken. Wait for fix release![/]");
                        break;
                }
            }
        }

        private static void DeleteSession()
        {
            throw new NotImplementedException();
        }

        private static void ChangeSessionData()
        {
            throw new NotImplementedException();
        }

        private static void StartNewSession()
        {
            throw new NotImplementedException();
        }

        private static void ViewAllSessions()
        {
            throw new NotImplementedException();
        }
    }
}

