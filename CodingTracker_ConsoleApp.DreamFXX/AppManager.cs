using Spectre.Console;

class AppManager
{
    private const string START_CODE_SESH = "Start recording actual Code compiling session";
    private const string END_CODE_SESH = "End or stop active Code compiling session(s)";
    
    private const string SAVE_ACTIVE_CODETIME_SESH = "Save your newly recorded Coding sessions";
    private const string VIEW_ALL_STORED_SESSIONS = "View all stored sessions and coding times";
    private const string CHANGE_TIME_IN_SESSION = "";
    private const string DELETE_CODE_SESH_DATA = "Delete your already saved Coding sessions and logs";

    private const string EXIT = "Close Application";

    private DatabaseManager dDatabaseManager;
    public AppManager(DatabaseManager databaseManager)
    {
        dDatabaseManager = databaseManager;
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
            CHANGE_TIME_IN_SESSION,
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
                ViewAllSessions();
                break;

            case CHANGE_TIME_IN_SESSION:
                ChangeCodingSession();
                break;
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
        throw new NotImplementedException();
    }

    private void EndSession()
    {
        throw new NotImplementedException();
    }

    private void SaveCodeTimeSession()
    {
        throw new NotImplementedException();
    }


    private void ViewAllSessions()
    {
        throw new NotImplementedException();
    }

    private void ChangeCodingSession()
    {
        throw new NotImplementedException();
    }

    private void DeleteCodingSession()
    {
        throw new NotImplementedException();
    }

}

