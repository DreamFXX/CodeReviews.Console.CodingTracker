using Spectre.Console;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.DreamFXX.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CodingTracker.DreamFXX
{
    public class DatabaseManager
    {
        private readonly string? _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var parameters = new[]
                {
                    new SqliteParameter("@date", date),
                    new SqliteParameter("@startTime", startTime),
                    new SqliteParameter("@endTime", endTime),
                    new SqliteParameter("@duration", duration)
                };
                connection.Execute(@"INSERT INTO MyCodingTracker (Date, StartTime, EndTime, Duration)
                                       VALUES(@date, @startTime, @endTime, @duration)");
            }
        }

        public void DeleteRecord(string? date)
        {
            var parameter = new SqliteParameter("@date", date);
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = @$"DELETE FROM MyCodingTracker
                                            WHERE Date = @date";
                connection.Execute(sql);
            }
        }

        public void UpdateRecord(string? oldDate, string? newDate, string? startTime, string? endTime, string? duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var parameters = new[]
                {
                    new SqliteParameter("@date", oldDate),
                    new SqliteParameter("@newDate", newDate),
                    new SqliteParameter("@startTime", startTime),
                    new SqliteParameter("@endTime", endTime),
                    new SqliteParameter("@duration", duration)
                };

                if (startTime is null && endTime is null)
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                             SET Date = @newDate
                                             WHERE Date = @oldDate");
                }
                else if (newDate is null)
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                             SET StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                             WHERE Date = @oldDate");
                }
                else
                {
                    connection.Execute(@"UPDATE MyCodingTracker
                                              SET Date = @newDate, StartTime = @startTime, EndTime = @endTime, Duration = @duration
                                              WHERE Date = @oldDate");
                }
            }
        }

        public List<CodingSession> ReadFromDb()
        {
            List<CodingSession> codingSessions = new();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"SELECT * FROM MyCodingTracker";
                using (var command = new SqliteCommand(sqlQuery, connection))
                using (var reader = command.ExecuteReader())

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new CodingSession
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Date = reader["Date"].ToString()!,
                                StartTime = reader["StartTime"].ToString()!,
                                EndTime = reader["EndTime"].ToString()!,
                                Duration = reader["Duration"].ToString()!
                            };

                            codingSessions.Add(model);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]No records found in the database.[/]");
                    }
            }
            return codingSessions;
        }

        public CodingSession? ReadSingleRecord(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM MyCodingTracker WHERE Id = @Id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CodingSession
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Date = reader["Date"].ToString()!,
                                StartTime = reader["StartTime"].ToString()!,
                                EndTime = reader["EndTime"].ToString()!,
                                Duration = reader["Duration"].ToString()!
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
