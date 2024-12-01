﻿using MyCodingTracker.Models;
using Spectre.Console;
using System.Reflection;

namespace MyCodingTracker
{
    public static class ProgramController
    {
        private static readonly DatabaseManager DbManager = new();
        private static readonly UserInput Input = new();

        private static void MainMenu()
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
            while (menuPrompt != "Close application")
            {
                switch (menuPrompt)
                {
                    case "View all tracked sessions":
                        DisplayInTable();
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
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid selection![/]");
                        break;
                }

                var backtoMenu = AnsiConsole.Confirm("Do you want to go back to the menu?");
                if (backtoMenu)
                {
                    MainMenu();
                }
            }
            Environment.Exit(0);
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
            var updateChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[green]Select the type of Data you want to change.[/][/]")
                .PageSize(5)
                .AddChoices<string>(new[]
                {
                    "Update Date",
                    "Update Start and End Time",
                    "Update All Attributes",
                    "Go Back to Main Menu"
                }));

            if (updateChoice != "Go Back to Main Menu")
            {
                Console.Clear();
                return;
            }
            SelectRecordToUpdate(updateChoice);
        }

        private static void SelectRecordToUpdate(string updateChoice)
        {
            AnsiConsole.Clear();
            DisplayInTable();

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
                DisplayInTable();
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
            throw new NotImplementedException();
        }

        public static void DisplayInTable()
        {
            var sessions = DbManager.ReadFromDb();
            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[red]No records found![/]");
                return;
            }
            var table = new Table()
               .Border(TableBorder.Rounded)
               .BorderColor(Color.Blue)
               .Title("[yellow bold]Your tracked coding sessions[/]");
            var type = typeof(string);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                table.AddColumn(property.Name);
            }

            foreach (var dataItem in data)
            {
                table.AddRow(properties.Select(p => p.GetValue(dataItem)?.ToString() ?? string.Empty).ToArray());
            }

            AnsiConsole.Write(table.Expand());
        }
        }
}
