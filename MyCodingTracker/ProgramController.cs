using System.Data.SqlTypes;
using System.Reflection.Metadata;
using Spectre.Console;
using SQLitePCL;

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
            StartProgram(menuPrompt);
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
                        GetRecordsToInsert();
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
                    default:
                        AnsiConsole.MarkupLine("[red]This menu route is broken. Wait for fix release![/]");
                        break;
                }
            }
        }

        private static void GetRecordsToInsert()
        {
            AnsiConsole.Clear();

            string date = Input.GetDate();

            string startTime = Input.GetStartTime();

            string endTime = Input.GetEndTime();

            string duration = Input.GetDuration();

            DbManager.InsertRecord(date, startTime, endTime, duration);

            ViewRecords();
        }

        private static void DisplayDeleteContextMenu()
        {
            Console.WriteLine("b to Go Back");
            Console.WriteLine("d to Delete Record: ");
        }

        private static void GetRecordsToDelete()
        {
            Console.Clear();
            ViewRecords();

            DisplayDeleteContextMenu();
            string choice = Console.ReadLine();
            while (choice != "b")
            {
                switch (choice)
                {
                    case "d":
                        DbManager.DeleteRecord(Input.GetDate());
                        ViewRecords();
                        break;
                    default:
                        Console.WriteLine("Invalid Choice!");
                        ViewRecords();
                        break;
                }
                DisplayDeleteContextMenu();
                choice = Console.ReadLine();
            }
            ViewRecords();
        }

        private static void DisplayUpdateContextMenu()
        {
            string[] types;
            var updateMenu = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[underline][green]Select type of Data you are going to change[/][/]")
                .PageSize(5)
                .AddChoices<string>(new string[]
                {
                "Date", "Start and End time", "Change all attributes of session", "", "Go back to main menu"
                }
                ));
        }

        private static void SelectRecordToUpdate()
        {
            Console.Clear();
            ViewAllRecords();

            string? newDate, startTime, endTime, duration;
            DbManager.ReadFromDb();

            string oldDate = Input.GetDate();

            DisplayUpdateContextMenu();
            string choice = Console.ReadLine();
            while (choice != "b")
            {
                switch (choice)
                {
                    case "d":
                        newDate = Input.GetDate();
                        DbManager.UpdateRecord(oldDate, newDate, null, null, null);
                        break;
                    case "t":
                        startTime = Input.GetStartTime();
                        endTime = Input.GetEndTime();
                        duration = Input.GetDuration();
                        DbManager.UpdateRecord(oldDate, null, startTime, endTime, duration);
                        break;
                    case "a":
                        newDate = Input.GetDate();
                        startTime = Input.GetStartTime();
                        endTime = Input.GetEndTime();
                        duration = Input.GetDuration();
                        DbManager.UpdateRecord(oldDate, newDate, startTime, endTime, duration);
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
                ViewAllRecords();
                DisplayUpdateContextMenu();
                choice = Console.ReadLine();
            }

            Console.Clear();
        }

        private static void ViewRecords()
        {
            DisplayRecords.View();
        }

        private static void ViewAllRecords()
        {
            DisplayRecords.ViewAll();
        }
    }
}
