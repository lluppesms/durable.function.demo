namespace Durable.Demo.Fanout;

public static class Trigger
{
    private static string LogDataSource = Constants.DataSource.Fanout.Trigger;

	[OpenApiOperation(operationId: "Fanout_Trigger", tags: new[] { "name" }, Summary = "Start Fanout Orchestration", Description = "Start Fanout Orchestration", Visibility = OpenApiVisibilityType.Important)]
	[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
	[FunctionName("Fanout_Trigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        MyLogger.InitializeLogger(log);

        var duration = Common.ParseIntegerFromRequest(req, "duration", 60);
        var instanceId = await starter.StartNewAsync("Fanout_Orchestrator", null, duration);
        MyLogger.LogInfo($"{LogDataSource} started orchestration with ID = '{instanceId}'.", LogDataSource);
        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
