using Spectre.Console;
using System.Globalization;

class AppManager
{
    private const string START_CODE_SESH = "Start recording actual code compiling session.";
    private const string END_CODE_SESH = "End or stop active code compiling session(s).";

    private const string SAVE_ACTIVE_CODETIME_SESH = "Save your newly recorded coding sessions.";
    private const string VIEW_ALL_STORED_SESSIONS = "View all stored sessions and coding times.";
    private const string CHANGE_TIME_IN_SESSION = "Change properties of a record.";
    private const string DELETE_CODE_SESH_DATA = "Delete your already saved coding sessions and logs";

    private const string EXIT = "Close Application";

    private DatabaseManager _databaseManager;
    public AppManager(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void AppStart()
    {
        while (true)
        {
            MainMenu();
        }

    }

    private void MainMenu()
    {
        AnsiConsole.Clear();

        List<string> menuRoutes = new List<string>
        {
            START_CODE_SESH,
            END_CODE_SESH,
            SAVE_ACTIVE_CODETIME_SESH,

            VIEW_ALL_STORED_SESSIONS,
            // CHANGE_TIME_IN_SESSION,
            DELETE_CODE_SESH_DATA,


            EXIT,
        };

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Welcome in Coding Session Tracker - Log your spent time with Coding!")
            .PageSize(9)
            .MoreChoicesText("[grey](Move with arrows, up or down to show more options.)[/]")
            .AddChoices(menuRoutes)
            );
        try
        {
            MenuRouter(selected);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]An error has been detected. Restart or fix your application, read an error message below.[/]\n\n[red]{e.Message}[/]");
            Console.ReadKey();
        }
    }

    private void MenuRouter(string selected)
    {
        switch (selected)
        {
            case START_CODE_SESH:
                StartSession();
                break;
            case END_CODE_SESH:
                EndSession();
                break;
            case SAVE_ACTIVE_CODETIME_SESH:
                SaveCodeTimeSession();
                break;
            case VIEW_ALL_STORED_SESSIONS:
                ViewCodingSessions();
                break;

            //case CHANGE_TIME_IN_SESSION:
            //    ChangeCodingSession();
            //    break;
            case DELETE_CODE_SESH_DATA:
                DeleteCodingSession();
                break;
            case EXIT:
                Environment.Exit(0);
                break;
        }
    }

    private void StartSession()
    {
        if (_databaseManager.ActiveCodingSession != null)
        {
            AnsiConsole.MarkupLine("[red]Error! -> You have already at least one active session. Press any key to continue.[/]");
            Console.ReadKey();
            return;
        }
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("Starting new code session recording.");
        CodingSession codingSession = new CodingSession()
        {
            StartTime = DateTime.UtcNow
        };

        _databaseManager.CreateCodeSession(codingSession);
        AnsiConsole.MarkupLine("[yellow]Coding session started![/]");
        AnsiConsole.MarkupLine($"Start time of this session: [underline]{codingSession.StartTime}[/]");
        AnsiConsole.MarkupLine("\nPress any key to continue...");
        Console.ReadKey();

    }

    private void EndSession()
    {
        CodingSession? codingSession = _databaseManager.ActiveCodingSession();
        if (codingSession == null)
        {
            AnsiConsole.MarkupLine("[red]Error! -> There is no active coding session to end.[/]");
            Console.ReadKey();
            return;
        }

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("Closing actual coding session.");

        codingSession.EndTime = DateTime.Now;
        codingSession.ActiveStat = false;
        _databaseManager.UpdateCodingSession(codingSession);

        AnsiConsole.MarkupLine("[yellow]Coding session ended.[/]");
        AnsiConsole.MarkupLine($"End time of session: [underline]{codingSession.EndTime}[/]");
        AnsiConsole.MarkupLine($"\nDuration: [yellow]{codingSession.Duration}[/]");
        Console.ReadKey();
    }

    private void SaveCodeTimeSession()
    {
        AnsiConsole.Clear();


        AnsiConsole.MarkupLine("[springgreen2_1]Coding session logged![/]");
    }

    private void ListCodingSessions(List<CodingSession> codingSessions)
    {
        Table sessionsTable = new Table();
        sessionsTable.Title(new TableTitle("[underline]List of all recorded sessions of coding[/]"));
        sessionsTable.AddColumns("Id, Name, StartTime, EndTime, Duration");
        sessionsTable.Columns[0].Width = 5;

        foreach (CodingSession codingSession in codingSessions)
        {
            Style? activeStat = codingSession.ActiveStat ? Style.Parse("chartreuse2") : null;

        }
    }

    private void ViewCodingSessions()
    {
        List<CodingSession> codingSessions = _databaseManager.GetAllSessionsData();
        AnsiConsole.Clear();

        ListCodingSessions(codingSessions);
        AnsiConsole.MarkupLine("[springgreen2_1]Press any key to get back to the menu.[/]");
        Console.ReadKey();
    }


    // !!
    private void DeleteCodingSession()
    {
        AnsiConsole.Clear();

        List<CodingSession> codingSessions = _databaseManager.GetAllSessionsData();
        ListCodingSessions(codingSessions);

    }
}

