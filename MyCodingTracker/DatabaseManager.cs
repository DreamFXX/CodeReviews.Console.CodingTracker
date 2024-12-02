using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;
using System.Reflection;
using MyCodingTracker.Models;
using Spectre.Console;

namespace MyCodingTracker
{
    internal class DatabaseManager
    {
        private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(@"CREATE TABLE IF NOT EXISTS MyCodingTracker (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        StartTime TEXT,
                                        EndTime TEXT,
                                        Duration TEXT)");
            }
        }

        public void InsertRecord(string date, string startTime, string endTime, string duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(@$"INSERT INTO MyCodingTracker (Date, StartTime, EndTime, Duration)
                                       VALUES('{date}', '{startTime}', '{endTime}', '{duration}')");
            }
        }

        public void DeleteRecord(string date)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string sql = @$"DELETE FROM MyCodingTracker
                                            WHERE Date = '{date}'";
                connection.Execute(sql);
            }
        }

        public void UpdateRecord(string oldDate, string? newDate, string? startTime, string? endTime, string? duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    if (startTime is null && endTime is null)
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                                SET Date = '{newDate}'
                                                WHERE Date = '{oldDate}'");
                    }
                    else if (newDate is null)
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                                SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'
                                                WHERE Date = '{oldDate}'");
                    }
                    else
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                               SET Date = '{newDate}', StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'
                                               WHERE Date = '{oldDate}'");
                    }
                }
            }
        }

        public List<CodingSession> ReadFromDb()
        {
            List<CodingSession> codingSessions = new();

            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var command = new SqliteCommand("SELECT * FROM MyCodingTracker", connection))
                using (var reader = command.ExecuteReader()) 

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var models = new Models.CodingSession();
                            models.Date = (string)reader["Date"];
                            models.StartTime = (string)reader["StartTime"];
                            models.EndTime = (string)reader["EndTime"];
                            models.Duration = (string)reader["Duration"];

                            codingSessions.Add(models);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("\n[red]ERROR - You don't have any records![/]\n");
                    }
            }
            return codingSessions;
        }
        }
    }
