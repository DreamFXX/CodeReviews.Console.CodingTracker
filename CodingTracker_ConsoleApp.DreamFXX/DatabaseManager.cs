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

        using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
        {
            conn.Execute("CREATE TABLE IF NOT EXISTS Codingtime_Data (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, TimeStarted TEXT, TimeEnded TEXT, Duration INT, ActiveStat INTEGER DEFAULT 1)");
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