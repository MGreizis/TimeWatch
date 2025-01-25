using System;
using System.Timers;
using Dapper;
using Microsoft.Data.Sqlite;

namespace TimeWatcher.Services;

public class AppUsageMonitor
{
  private readonly System.Timers.Timer _timer;
  private string _currentApp;
  private DateTime _startTime;

  public AppUsageMonitor()
  {
    _timer = new System.Timers.Timer(1000);
    _timer.Elapsed += TrackActiveWindow;
    _currentApp = string.Empty;
    _startTime = DateTime.Now;
  }

  public void Start() => _timer.Start();
  public void Stop() => _timer.Stop();

  private void TrackActiveWindow(object? sender, ElapsedEventArgs e)
  {
    string activeApp = ActiveWindowTracker.GetActiveWindowTitle();

    if (activeApp != _currentApp)
    {
      if (!string.IsNullOrEmpty(_currentApp))
      {
        LogAppUsage(_currentApp, _startTime, DateTime.Now);
      }

      _currentApp = activeApp;
      _startTime = DateTime.Now;
    }
  }

  private void LogAppUsage(string appName, DateTime startTime, DateTime endTime)
  {
    using var connection = new SqliteConnection("Data Source=ScreenTime.db");
    connection.Open();

    string query = @"
      INSERT INTO AppUsage (AppName, StartTime, EndTime)
      VALUES (@AppName, @StartTime, @EndTime)
    ";

    connection.Execute(query, new { AppName = appName, StartTime = startTime, EndTime = endTime });
  }
}