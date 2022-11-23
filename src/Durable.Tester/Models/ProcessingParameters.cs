namespace Durable.Test.Harness.Models;

public class ProcessingParameters
{
    /// <summary>
    /// Environment: DEV/QA/PROD
    /// </summary>
    public string EnvironmentCode { get; set; }

    /// <summary>
    /// URL to Call
    /// </summary>
    public string FunctionUrl { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Initializer
    /// </summary>
    public ProcessingParameters()
    {
        EnvironmentCode = Constants.Environments.Dev;
        FunctionUrl = string.Empty;
        PhoneNumber = string.Empty;
    }

    /// <summary>
    /// Displays the current values to the user
    /// </summary>
    public void DisplayConfigurationValues()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"\nConfiguration:");
        sb.AppendLine($"  Environment:             {EnvironmentCode}");
        sb.AppendLine($"  FunctionUrl:             {FunctionUrl}");
        sb.AppendLine($"  PhoneNumber:             {PhoneNumber}");
        sb.AppendLine($"  Log File:                {Constants.GetLogFileName()}");
        sb.AppendLine(string.Empty);
        Utilities.DisplayMessage(sb.ToString(), ConsoleColor.Magenta);
    }
}
