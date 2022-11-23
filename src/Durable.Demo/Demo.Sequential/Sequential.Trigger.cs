namespace Durable.Demo.Sequential;

public static class Sequential_Trigger
{
    private static string DataSource = Constants.DataSource.Sequential.Trigger;
    
    [FunctionName("Sequential_Trigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log
    )
    {
        MyLogger.InitializeLogger(log);

        var duration = Common.ParseIntegerFromRequest(req, "duration", 60);
        var instanceId = await starter.StartNewAsync("Sequential_CreateTasks", null, duration);
        MyLogger.LogInfo($"{DataSource} started orchestration with ID = '{instanceId}'.", DataSource);
        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
