using Spectre.Console;

class AppManager
{
    private const string START_CODE_SESH = "Start recording actual Code compiling session";
    private const string END_CODE_SESH = "End or stop active Code compiling session(s)";
    
    private const string SAVE_ACTIVE_CODETIME_SESH = "Save your newly recorded Coding sessions";
    private const string VIEW_ALL_STORED_SESSIONS = "View all stored sessions and coding times";
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
            DELETE_CODE_SESH_DATA,

            EXIT,
        };    
    }

    private void menuRouter()
    {
        throw new NotImplementedException();
    }
}

