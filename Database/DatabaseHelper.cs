using Microsoft.Data.Sqlite;
using System;

namespace TimeWatcher.Database;

public static class DatabaseHelper
{
  private const string ConnectionString = "Data Source=ScreenTime.db";

  public static void InitializeDatabase()
  {
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    string tableCreationQuery = @"
      CREATE TABLE IF NOT EXISTS AppUsage (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        AppName TEXT NOT NULL,
        StartTime TEXT NOT NULL,
        EndTime TEXT NOT NULL
      );
    ";

    using var command = new SqliteCommand(tableCreationQuery, connection);
    command.ExecuteNonQuery();
  }
}