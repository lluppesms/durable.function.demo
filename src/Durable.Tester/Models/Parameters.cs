namespace Durable.Test.Harness.Models;

public class Parameters
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
}
