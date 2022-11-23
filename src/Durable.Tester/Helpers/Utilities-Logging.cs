namespace Durable.Test.Harness.Helpers;

public static partial class Utilities
{
    /// <summary>
    /// Write message to log file
    /// </summary>
    public static void WriteToLogFile(string message)
    {
        var extraLineBreak = string.Empty;
        try
        {
            var logFilePath = GetApplicationDirectory() + Constants.GetLogFileName();
            using var logFile = new StreamWriter(logFilePath, true);
            if (message.StartsWith("******")) { extraLineBreak = "\n"; }
            logFile.WriteLine($"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture)} {extraLineBreak}{message.Trim()}");
        }
        catch
        {
            // ignore it...
        }
    }

    /// <summary>
    /// Reset log file
    /// </summary>
    public static void ResetLogFile()
    {
        try
        {
            var logFilePath = GetApplicationDirectory() + Constants.GetLogFileName();
            using var logFile = new StreamWriter(logFilePath, false);
            logFile.WriteLine($"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture)} Log file reset...");
        }
        catch
        {
            // ignore it...
        }
    }

    /// <summary>
    /// Remove old log files
    /// </summary>
    public static void RemoveOldLogFile()
    {
        try
        {
            DisplayMessage($"    Removing old log files older than 3 days...", ConsoleColor.Yellow);
            var logFileDirectory = GetApplicationDirectory();
            var filePaths = Directory.GetFiles(logFileDirectory, "*.log");
            var fileName0 = Constants.LogFileName.Replace(".log", $"{DateTime.Today.ToString("-yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
            var fileName1 = Constants.LogFileName.Replace(".log", $"{DateTime.Today.AddDays(-1).ToString("-yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
            var fileName2 = Constants.LogFileName.Replace(".log", $"{DateTime.Today.AddDays(-2).ToString("-yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
            var fileName3 = Constants.LogFileName.Replace(".log", $"{DateTime.Today.AddDays(-3).ToString("-yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
            foreach (var filePath in filePaths)
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Name != fileName0 && fileInfo.Name != fileName1 && fileInfo.Name != fileName2 && fileInfo.Name != fileName3)
                {
                    DisplayMessage($"      Found old log file {fileInfo.Name} -- deleting it!", ConsoleColor.Red);
                    File.Delete(filePath);
                }
            }
        }
        catch
        {
            // ignore it...
        }
    }
}
