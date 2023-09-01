namespace Durable.Demo.Sequential;

public static class Sequential_Trigger
{
    private static string DataSource = Constants.DataSource.Sequential.Trigger;

	[OpenApiOperation(operationId: "Sequential_Trigger", tags: new[] { "name" }, Summary = "Start Sequential Operation", Description = "Start Sequential Operation", Visibility = OpenApiVisibilityType.Important)]
	[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
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
