using Microsoft.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;

namespace MyCodingTrack
{
    internal class DatabaseManager
    {
        private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(@"CREATE TABLE IF NOT EXISTS MyCodingTracker_Data (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        StartTime TEXT,
                                        EndTime TEXT,
                                        Duration TEXT)");
            }
        }
    }
}
