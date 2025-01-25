using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeWatcher.Services;

public class ActiveWindowTracker
{
  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern int GetWindowText(IntPtr hWnd, StringBuilder IpString, int nMaxCount);

  public static string GetActiveWindowTitle()
  {
    const int nChars = 256;
    StringBuilder buffer = new StringBuilder(nChars);
    IntPtr handle = GetForegroundWindow();

    if (GetWindowText(handle, buffer, nChars) > 0)
    {
      return buffer.ToString();
    }

    return "Unknown";
  }
}