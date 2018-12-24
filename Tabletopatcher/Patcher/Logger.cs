namespace Fougerite.Patcher
{
  using System;
  using System.IO;

  public static class Logger
  {
    public static void Clear()
    {
      if (File.Exists("patcherLog.txt"))
      {
        File.Delete("patcherLog.txt");
      }
    }

    public static void Log(string msg)
    {
      Console.WriteLine(msg);
      File.AppendAllText("patcherLog.txt", "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] " + msg + "\r\n");
    }

    public static void Log(Exception ex)
    {
      Console.WriteLine("\n======| EX INFO |======");
      Console.WriteLine(ex);
      Console.WriteLine("======| ======= |======\n");
      File.AppendAllText("patcherLog.txt", "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] " + ex + "\r\n");
    }
  }
}

