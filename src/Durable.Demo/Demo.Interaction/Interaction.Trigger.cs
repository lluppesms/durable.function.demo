// ------------------------------------------------------------------------------------------------------------------------
// Interaction_Trigger: Trigger that starts the verification/update process
// ------------------------------------------------------------------------------------------------------------------------
namespace Durable.Demo.Interaction;
public static class Interaction_Trigger
{
    private static string LogDataSource = Constants.DataSource.Interaction.Trigger;

	[OpenApiOperation(operationId: "Interaction_Trigger", tags: new[] { "name" }, Summary = "Start Interaction Orchestration", Description = "Start Interaction Orchestration", Visibility = OpenApiVisibilityType.Important)]
	[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
	[FunctionName("Interaction_Trigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log, Microsoft.Azure.WebJobs.ExecutionContext executionContext
	)
    {
		MyLogger.Initialize_And_Log(log, $"{LogDataSource} started {executionContext.FunctionName}.", LogDataSource);

		// if you don't have Twilio set up in configuration variables, then use a Mock Twilio function
		var phoneNumber = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TwilioPhoneNumber")) ? await Common.ParseRequestBodyAsync(req) : Constants.MockPhoneNumber;
        var instanceId = await starter.StartNewAsync("Interaction_Orchestration", null, phoneNumber);
        MyLogger.LogInfo($"{LogDataSource} started orchestration with ID = '{instanceId}'.", LogDataSource);
        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
