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
                        ViewAllRecords();
                        break;
                    case "Start a new session record":
                        GetRecordsToInsert();
                        break;
                    case "Change an existing session data":
                        ChangeSessionData();
                        break;
                    case "Delete coding session data":
                        DeleteContextMenu();
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
            AnsiConsole.MarkupLine("[yellow]Yay! Record has been saved.");
            ViewRecords();
        }

        private static void DisplayUpdateContextMenu()
        {
            string[] types;
            var updateChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[underline][green]Select type of Data you are going to change[/][/]")
                .PageSize(5)
                .AddChoices<string>(new[]
                {
                    "Update Date",
                    "Update Start and End Time",
                    "Update All Attributes",
                    "Go Back to Main Menu"
                }));

            if (updateChoice == "Go Back to Main Menu")
            {
                Console.Clear();
                return;
            }
            SelectRecordToUpdate(updateChoice);
        }

        private static void SelectRecordToUpdate(string updateChoice)
        {
            Console.Clear();
            ViewAllRecords();

            string oldDate = Input.GetDate();
            DisplayUpdateContextMenu();

                switch (updateChoice)
                {
                    case "Update Date":
                        string newDate = Input.GetDate();
                        DbManager.UpdateRecord(oldDate, newDate, null, null, null);
                        break;
                    case "Update Start and End Time":
                        string startTime = Input.GetStartTime();
                        string endTime = Input.GetEndTime();
                        string duration = Input.GetDuration();
                        DbManager.UpdateRecord(oldDate, null, startTime, endTime, duration);
                        break;
                    case "Update All Attributes":
                        newDate = Input.GetDate();
                        startTime = Input.GetStartTime();
                        endTime = Input.GetEndTime();
                        duration = Input.GetDuration();
                        DbManager.UpdateRecord(oldDate, newDate, startTime, endTime, duration);
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid selection. Returning to the main menu.[/]");
                        break;
                }
                AnsiConsole.MarkupLine("[green]Record updated successfully![/]");
                ViewAllRecords();
        }

        private static void DeleteContextMenu()
        {
            AnsiConsole.Clear();
            var confirmation = AnsiConsole.Confirm("[green]Do you really want to delete record?[/]");
            if (confirmation)
            {
                Console.Clear();
                ViewRecords();

                string dateToDelete = Input.GetDate();
                DbManager.DeleteRecord(dateToDelete);

                AnsiConsole.MarkupLine($"[green]Record for {dateToDelete} deleted successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Deleting was cancelled.[/]");
            }
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
