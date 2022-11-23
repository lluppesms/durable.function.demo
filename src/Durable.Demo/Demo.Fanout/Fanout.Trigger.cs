namespace Durable.Demo.Fanout;

public static class Trigger
{
    private static string DataSource = Constants.DataSource.Fanout.Trigger;
    [FunctionName("Fanout_Trigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        MyLogger.InitializeLogger(log);

        var duration = Common.ParseIntegerFromRequest(req, "duration", 60);
        var instanceId = await starter.StartNewAsync("Fanout_Orchestrator", null, duration);
        MyLogger.LogInfo($"{DataSource} started orchestration with ID = '{instanceId}'.", DataSource);
        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
