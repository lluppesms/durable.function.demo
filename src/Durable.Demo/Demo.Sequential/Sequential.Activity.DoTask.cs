namespace Durable.Demo.Sequential;
using Durable.Demo.Sequential.Data;

public static class DoTask
{
    private static string DataSource = Constants.DataSource.Sequential.DoWork;

    [FunctionName("Sequential_DoTask")]
    public static string Run([ActivityTrigger] WorkStep step, ILogger log)
    {
        MyLogger.InitializeLogger(log);
        MyLogger.LogInfo($"{DataSource} was activated for {step.stepName}!", DataSource);
        Thread.Sleep(step.secondsToProcess * 1000);
        return $"Completed {step.stepName}!";
    }
}
