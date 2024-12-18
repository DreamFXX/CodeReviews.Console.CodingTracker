using System.Configuration;
using CodingTracker.DreamFXX.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.DreamFXX;

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
            throw new ArgumentException(
                "Error - You must provide a date, start time, and end time in displayed format.");

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(
                @"INSERT INTO MyCodingTracker (Date, StartTime, EndTime, Duration)
                          VALUES (@date, @startTime, @endTime, @duration)",
                new { date, startTime, endTime, duration });
        }
    }

    public void DeleteRecord(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            connection.Execute(@"DELETE FROM MyCodingTracker
                                            WHERE Id = @id", new { id });
        }
    }

    public void UpdateRecord(int id, string? newDate, string? startTime, string? endTime, string? duration)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            if (startTime is null && endTime is null)
                connection.Execute(@"UPDATE MyCodingTracker
                                         SET Date = @newDate
                                         WHERE Id = @id", new { newDate, id });
            else if (newDate is null)
                connection.Execute(@"UPDATE MyCodingTracker
                                         SET StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                         WHERE Id = @id", new { startTime, endTime, duration, id });
            else
                connection.Execute(@"UPDATE MyCodingTracker
                                          SET Date = @newDate, StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                          WHERE Id = @id",
                    new { newDate, startTime, endTime, duration, id });
        }
    }

    public List<CodingSession> ReadFromDb()
    {
        var connection = new SqliteConnection(_connectionString);
        var sqlQuery = "SELECT * FROM MyCodingTracker";
        var codingSessions = connection.Query<CodingSession>(sqlQuery);
        return codingSessions.ToList();
    }

    public CodingSession? ReadSingleRecord(int id)
    {
        var connection = new SqliteConnection(_connectionString);
        var query = "SELECT * FROM MyCodingTracker WHERE Id = @id";
        var parameters = new
        {
            Id = id
        };

        var codingSessions = connection.QuerySingleOrDefault<CodingSession>(query, parameters);
        return codingSessions;
    }
}