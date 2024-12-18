using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.DreamFXX.Models;

namespace CodingTracker.DreamFXX
{
    public class DatabaseManager
    {
        private readonly string? _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        internal UserInput Input = new();

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"CREATE TABLE IF NOT EXISTS MyCodingTracker (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        StartTime TEXT,
                                        EndTime TEXT,
                                        Duration TEXT)");
            }
        }

        public void InsertRecord(string? date, string? startTime, string? endTime, string duration)
        {
            if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(startTime) ||
                string.IsNullOrWhiteSpace(endTime))
            {
                throw new ArgumentException(
                    "Error - You must provide a date, start time, and end time in displayed format.");
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(
                    @"INSERT INTO MyCodingTracker (Date, StartTime, EndTime, Duration)
                          VALUES (@date, @startTime, @endTime, @duration)",
                    new { date, startTime, endTime, duration });
            }
        }

        public void DeleteRecord(string? date)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(@"DELETE FROM MyCodingTracker
                                            WHERE Date = @date", new { date });
            }
        }

        public void UpdateRecord(string? oldDate, string? newDate, string? startTime, string? endTime, string? duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                if (@startTime is null && @endTime is null)
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                             SET Date = @newDate
                                             WHERE Date = @oldDate", new { newDate, oldDate });
                }
                else if (@newDate is null)
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                             SET StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                             WHERE Date = @oldDate", new { startTime, endTime, duration, oldDate });
                }
                else
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                              SET Date = @newDate, StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                              WHERE Date = @oldDate",
                        new { newDate, startTime, endTime, duration, oldDate });
                }
            }
        }

        public List<CodingSession> ReadFromDb()
        {
            var connection = new SqliteConnection(_connectionString);
            var sqlQuery = "SELECT * FROM MyCodingTracker";
            var codingSessions = connection.Query<CodingSession>(sqlQuery);
            return codingSessions.ToList();

        }

        public CodingSession? ReadSingleRecord(string? date)
        {
            var connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM MyCodingTracker WHERE Date = @date";
            var parameters = new
            {
                Date = date
            };

            var codingSessions = connection.QuerySingle<CodingSession>(query, parameters);
            return codingSessions;
        }
    }
}
