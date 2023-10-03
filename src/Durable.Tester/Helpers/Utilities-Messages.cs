namespace Durable.Test.Harness.Helpers;

public static partial class Utilities
{
    /// <summary>
    /// Display a console message
    /// </summary>
    public static void DisplayMessage(string message)
    {
        DisplayMessage(message, ConsoleColor.White);
    }

    /// <summary>
    /// Display an error message in red
    /// </summary>
    public static void DisplayErrorMessage(string message)
    {
        DisplayMessage(message, ConsoleColor.Red);
    }

    /// <summary>
    /// Display an exception's error message in red
    /// </summary>
    public static void DisplayError(Exception ex)
    {
        DisplayMessage(GetExceptionMessage(ex), ConsoleColor.Red);
    }

    /// <summary>
    /// Display a console message in a box
    /// </summary>
    public static void DisplayMessageInABox(string message)
    {
        DisplayMessageInABox(message, ConsoleColor.White);
    }

    /// <summary>
    /// Display a console message in a box
    /// </summary>
    public static void DisplayMessageInABox(string message, ConsoleColor color)
    {
        DisplayMessage(
            "********************************************************************************" + 
            "\n* " + message + 
            "\n********************************************************************************",
            color);
    }

    /// <summary>
    /// Display a console message with colors without logging
    /// </summary>
    public static void DisplayConsoleOnlyMessage(string message, ConsoleColor color)
    {
        DisplayMessage(message, color, false);
    }

    /// <summary>
    /// Display a time stamped console message with colors and send to log file
    /// </summary>
    public static void DisplayTimeStampedMessage(string message, ConsoleColor color, bool writeToLog = true)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"{DateTime.Now:hh:mm:ss} {message}");
        Console.ForegroundColor = previousColor;
        //Console.ResetColor();
        if (writeToLog)
        {
            WriteToLogFile(message);
        }
    }

    /// <summary>
    /// Display a console message with colors and send to log file
    /// </summary>
    public static void DisplayMessage(string message, ConsoleColor color, bool writeToLog = true)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = previousColor;
        //Console.ResetColor();
        if (writeToLog)
        {
            WriteToLogFile(message);
        }
    }

    /// <summary>
    /// Display a console message with colors and send to log file
    /// </summary>
    public static void DisplayClippyErrorMessage(string message, bool writeToLog = true)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(
            "********************************************************************************" +
            "\n* " + message +
            "\n********************************************************************************");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(
            "         /\n" +
            "        / \n" +
            "    __ /  \n" +
            "   /  \\ \n" +
            "   |  |  \n" +
            "   @  @  \n" +
            "   |  |  \n" +
            "   || |/ \n" +
            "   || || \n" +
            "   |\\_/|\n" +
            "   \\___/\n"
            );
        Console.ForegroundColor = previousColor;
        //Console.ResetColor();
        if (writeToLog)
        {
            WriteToLogFile(message);
        }
    }

    /// <summary>
    /// Combines all the inner exception messages into one string
    /// </summary>
    public static string GetExceptionMessage(Exception ex)
    {
        var message = string.Empty;
        if (ex == null)
        {
            return message;
        }
        if (ex.Message != null)
        {
            message += ex.Message;
        }
        if (ex.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.Message != null)
        {
            message += " " + ex.InnerException.Message;
        }
        if (ex.InnerException.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.InnerException.Message != null)
        {
            message += " " + ex.InnerException.InnerException.Message;
        }
        if (ex.InnerException.InnerException.InnerException == null)
        {
            return message;
        }
        if (ex.InnerException.InnerException.InnerException.Message != null)
        {
            message += " " + ex.InnerException.InnerException.InnerException.Message;
        }
        return message;
    }

    /// <summary>
    /// Clear a console line
    /// </summary>
    public static void ClearCurrentConsoleLine()
    {
        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    /// <summary>
	/// Returns a sanitized connection string suitable for display on admin page
    /// </summary>
    public static string GetSanitizedConnectionString(string connection)
    {
        //// "DeviceConnectionString": "HostName=iothub123.azure-devices.net;DeviceId=test1;SharedAccessKey=Placeholder-E5Z6******=",
        //// "SQLConnectionString": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=Placeholder-myPassword";
        string noKey;
        if (string.IsNullOrEmpty(connection)) return string.Empty;
        var keyPos = connection.IndexOf("key=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0) {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
            return noKey;
        }
        keyPos = connection.IndexOf("pwd=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0)
        {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
            return noKey;
        }
        keyPos = connection.IndexOf("password=", StringComparison.OrdinalIgnoreCase);
        if (keyPos > 0)
        {
            noKey = string.Concat(connection.AsSpan(0, keyPos + 9), "...");
            return noKey;
        }
        return connection;
    }

    /// <summary>
    /// Display Completion message
    /// </summary>
    public static void DisplayCompletionMessage(Stopwatch timer, string message = "Completed in")
    {
        timer.Stop();
        Utilities.DisplayMessage($"    {message} {timer.ElapsedMilliseconds} milliseconds! {DateTime.Now:hh:mm:ss} ", ConsoleColor.Cyan);
    }
}
