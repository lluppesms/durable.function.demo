// ------------------------------------------------------------------------------------------------------------------------
// Interaction_Trigger: Trigger that starts the verification/update process
// ------------------------------------------------------------------------------------------------------------------------
namespace Durable.Demo.Interaction;
public static class Interaction_Trigger
{
    private static string DataSource = Constants.DataSource.Interaction.Trigger;

    [FunctionName("Interaction_Trigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log
    )
    {
        MyLogger.InitializeLogger(log);

        // if you don't have Twilio set up in configuration variables, then use a Mock Twilio function
        var phoneNumber = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TwilioPhoneNumber")) ? await Common.ParseRequestBodyAsync(req) : Constants.MockPhoneNumber;
        var instanceId = await starter.StartNewAsync("Interaction_Orchestration", null, phoneNumber);
        MyLogger.LogInfo($"{DataSource} started orchestration with ID = '{instanceId}'.", DataSource);
        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
