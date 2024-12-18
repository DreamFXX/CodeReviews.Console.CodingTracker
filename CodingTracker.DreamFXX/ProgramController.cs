using Spectre.Console;

namespace CodingTracker.DreamFXX;

public static class ProgramController
{
    private static readonly DatabaseManager DbManager = new();
    private static readonly UserInput Input = new();

    public static void StartProgram()
    {
        DbManager.CreateDatabase();

        var running = true;
        while (running)
        {
            var menuPrompt = MainMenu();

            switch (menuPrompt)
            {
                case "View all tracked sessions":
                    ViewAllRecords();
                    break;
                case "Start a new session record":
                    GetRecordsToInsert();
                    break;
                case "Change an existing session data":
                    DisplayUpdateContextMenu();
                    break;
                case "Delete coding session":
                    DeleteContextMenu();
                    break;
                case "Close application":
                    running = false;
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid selection! Press any key to go back to the menu.[/]");
                    Console.ReadKey();
                    break;
            }
        }

        AnsiConsole.MarkupLine("[green]Thank you for using Coding Time Tracker![/]");
        Console.ReadKey();
    }

    private static string MainMenu()
    {
        return
            AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Welcome in Coding Time Tracker![/]\n[underline][yellow]MAIN MENU[/][/]")
                    .PageSize(10)
                    .AddChoices("View all tracked sessions", "Start a new session record",
                        "Change an existing session data", "Delete coding session", "Close application"));
    }

    private static void GetRecordsToInsert()
    {
        AnsiConsole.Clear();

        var date = Input.GetDate();
        var startTime = Input.GetStartTime();
        var endTime = Input.GetEndTime();
        var duration = Input.GetDuration();

        DbManager.InsertRecord(date, startTime, endTime, duration);

        AnsiConsole.MarkupLine("[yellow]Yay! Record has been saved. Press any key to return to the main menu.[/]");
        Console.ReadKey();
    }

    private static void DisplayUpdateContextMenu()
    {
        var updateChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("[green]Select the type of Data you want to change.[/]")
            .PageSize(5)
            .AddChoices("Update Date", "Update Start and End Time", "Update All Attributes", "Go Back to Main Menu"));

        SelectRecordToUpdate(updateChoice);
    }

    private static void SelectRecordToUpdate(string updateChoice)
    {
        AnsiConsole.Clear();
        ViewAllRecords();

        var oldDate = Input.GetDate();

        switch (updateChoice)
        {
            case "Update Date":
                var newDate = Input.GetDate();
                DbManager.UpdateRecord(oldDate, newDate, null, null, null);
                break;
            case "Update Start and End Time":
                var startTime = Input.GetStartTime();
                var endTime = Input.GetEndTime();
                var duration = Input.GetDuration();
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
                MainMenu();
                break;
        }

        AnsiConsole.MarkupLine("[green]Record updated successfully![/]");
        Console.ReadKey();
    }

    private static void DeleteContextMenu()
    {
        AnsiConsole.Clear();
        var confirmation = AnsiConsole.Confirm("[green]Do you really want to delete records?[/]");
        if (confirmation)
        {
            Console.Clear();
            var dateToDelete = Input.GetDate();
            var record = DbManager.ReadSingleRecord(dateToDelete);
            if (record != null)
            {
                DbManager.DeleteRecord(dateToDelete);
                AnsiConsole.MarkupLine($"[green]Record for {dateToDelete} deleted successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]No record found for {dateToDelete}![/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Deleting was cancelled.[/]");
        }
    }

    public static void ViewAllRecords()
    {
        AnsiConsole.Clear();

        var records = DbManager.ReadFromDb();
        if (records.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found in the database.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (var record in records)
            table.AddRow(
                record.Id.ToString(),
                record.Date,
                record.StartTime,
                record.EndTime,
                record.Duration
            );

        AnsiConsole.Write(table);
        Console.ReadKey();
    }
}