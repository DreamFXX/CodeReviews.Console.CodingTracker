using System.Data;
using System.Data.SQLite;
using Dapper;

class DatabaseManager
{
    private string _connectionString = "";

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
        SqlMapper.AddTypeHandler(new TimeSpanHandler());

        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Execute("CREATE TABLE IF NOT EXISTS CodingtimeData (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, TimeStarted TEXT, TimeEnded TEXT, Duration INT, ActiveStat INTEGER DEFAULT 1)");
        }
    }

    public void CreateCodeSession(CodingSession session)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "INSERT INTO CodingtimeData (StartTime, EndTime, Duration, Name, ActiveStat) VALUES (@StartTime, @EndTime, @Duration, @Name, @ActiveStat)";
            connection.Execute(sqlQuery, session);
        }
    }

    public void UpdateCodingSession(CodingSession session)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "UPDATE CodingtimeData SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration, ActiveStat = @ActiveStat WHERE Id = @Id";
            connection.Execute(sqlQuery, session);
        }
    }

    public void DeleteCodingSession(int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "DELETE FROM CodingtimeData WHERE Id = @Id";
            connection.Execute(sqlQuery, new { Id = id });
        }
    }


    public CodingSession? ActiveCodingSession()
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "SELECT * FROM CodingtimeData WHERE ActiveStat = 1 LIMIT 1";
            return connection.QueryFirstOrDefault<CodingSession>(sqlQuery);
        }
    }

    public CodingSession GetSessionData(int id)
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "SELECT * FROM CodingtimeData WHERE Id = @Id";
            CodingSession codingSession = connection.QueryFirstOrDefault<CodingSession>(sqlQuery, new { Id = id });
            if (codingSession == null)
            {
                throw new Exception("Entered Id of a record specified by you was not found.");
            }
            return codingSession;
        }
    }

    public List<CodingSession> GetAllSessionsData()
    {
        using(SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string sqlQuery = "SELECT * FROM CodingtimeData ORDER BY Id DESC";
            return connection.Query<CodingSession>(sqlQuery).ToList();
        }
    }
}


class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
{
    public override TimeSpan Parse(object value)
    {
        Console.WriteLine(value);
        return TimeSpan.FromTicks((int)value);
    }

    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.Ticks;
    }
}