// ------------------------------------------------------------------------------------------------------------------------
// Interaction_Orchestration: Orchestrator function that performs the verification process, including managing timeouts and retries.
// ------------------------------------------------------------------------------------------------------------------------
namespace Durable.Demo.Interaction;
public static class Interaction_Orchestration
{
    private static string DataSource = Constants.DataSource.Interaction.Orchestration;

    [FunctionName("Interaction_Orchestration")]
    public static async Task<bool> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
    {
        try
        {
            var phoneNumber = context.GetInput<string>();
            log.LogInformation($"{DataSource} was activated with {phoneNumber}!");

            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber), "A phone number input is required.");
            }

            // if you don't have Twilio set up in configuration variables, then use a Mock Twilio function
            var challengeActivityName = string.Equals(phoneNumber, Constants.MockPhoneNumber) ? "Interaction_SendChallengeMock" : "Interaction_SendChallenge";

            var authorized = false;
            var challengeCode = await context.CallActivityAsync<int>(challengeActivityName, phoneNumber);
            using (var timeoutCts = new CancellationTokenSource())
            {
                // The user has 120 seconds to respond with the code they received in the SMS message.
                var expiration = context.CurrentUtcDateTime.AddSeconds(120);
                var timeoutTask = context.CreateTimer(expiration, timeoutCts.Token);
                for (int retryCount = 0; retryCount <= 3; retryCount++)
                {
                    var challengeResponseTask = context.WaitForExternalEvent<int>("SmsChallengeResponse");
                    var winner = await Task.WhenAny(challengeResponseTask, timeoutTask);
                    if (winner == challengeResponseTask)
                    {
                        // Got back a response -- compare it to the challenge code
                        if (challengeResponseTask.Result == challengeCode)
                        {
                            authorized = true;
                            break;
                        }
                    }
                    else
                    {
                        // Timeout expired
                        break;
                    }
                }
                if (!timeoutTask.IsCompleted)
                {
                    // All pending timers must be complete or canceled before the function exits.
                    timeoutCts.Cancel();
                }
            }
            if (authorized)
            {
                log.LogInformation($"{DataSource} Submitting work request for {phoneNumber}");
                await context.CallActivityAsync<string>("Interaction_DoWork", phoneNumber);
            }
            else
            {
                log.LogInformation($"{DataSource} SMS Verification failed for {phoneNumber}!");
            }
            return authorized;
        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            return false;
        }
    }
}
