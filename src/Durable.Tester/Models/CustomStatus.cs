namespace Durable.Test.Harness.Models;

public class CustomStatus
{
    public string message { get; set; }
    public string progress { get; set; }
    public int percentComplete { get; set; }
    public int currentStep { get; set; }
    public int totalSteps { get; set; }
}

