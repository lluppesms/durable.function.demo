// ------------------------------------------------------------------------------------------------------------------------
// SendChallenge: Activity function that sends a code via text message to the user
// ------------------------------------------------------------------------------------------------------------------------
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace Durable.Demo.Interaction;
public static class SendChallenge
{
    private static string DataSource = Constants.DataSource.Interaction.SendChallenge;

    [FunctionName("Interaction_SendChallenge")]
    public static int SendSmsChallenge([ActivityTrigger] string phoneNumber, ILogger log,
      [TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "%TwilioPhoneNumber%")] out CreateMessageOptions message)
    {
        MyLogger.InitializeLogger(log);
        MyLogger.LogInfo($"{DataSource} was activated with {phoneNumber}!", DataSource);

        var challengeCode = GetChallengeCode();
        MyLogger.LogInfo($"{DataSource} Sending verification code {challengeCode} to {phoneNumber}.", DataSource);

        message = new CreateMessageOptions(new PhoneNumber(phoneNumber));
        message.Body = $"Your verification code is {challengeCode:0000}";

        return challengeCode;
    }

    [FunctionName("Interaction_SendChallengeMock")]
    public static int SendSmsChallengeMock([ActivityTrigger] string phoneNumber, ILogger log)
    {
        MyLogger.InitializeLogger(log);
        MyLogger.LogInfo($"{DataSource} was activated with {phoneNumber}!", DataSource);

        var challengeCode = GetChallengeCode();
        MyLogger.LogInfo($"{DataSource} *MOCK* sending verification code {challengeCode} to {phoneNumber}.", DataSource);
        return challengeCode;
    }

    private static int GetChallengeCode()
    {
        // Get a random number generator with a random seed (not time-based)
        var rand = new Random(Guid.NewGuid().GetHashCode());
        int challengeCode = rand.Next(10000);
        return challengeCode;
    }
}