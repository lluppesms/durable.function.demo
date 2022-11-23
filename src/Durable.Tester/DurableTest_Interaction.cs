namespace Durable.Test.Harness;

public partial class DurableTest
{
    private async Task<bool> Execute_UserInteractionTest()
    {
        var timer = new Stopwatch();
        timer.Start();
        try
        {
            Utilities.DisplayMessage($"\nStarting User Interaction Durable Function...", ConsoleColor.Green);
            var url = $"{config.FunctionUrl}/{Constants.FunctionUrlSuffix.Interaction}";
            var phoneNumber = GetPhoneNumberFromUser();

            var durableInstance = await Utilities.StartDurableFunction(url, phoneNumber);
            if (durableInstance != null)
            {
                Utilities.DisplayMessage($"    Interaction ID: {durableInstance.id}", ConsoleColor.Blue);

                var status = await Utilities.CheckDurableStatus(durableInstance.statusQueryGetUri, true);

                var verificationCode = GetVerificationCodeFromUser();
                if (!string.IsNullOrEmpty(verificationCode))
                {
                    var verificationResponse = await Utilities.SendDurableEvent(durableInstance.sendEventPostUri, "SmsChallengeResponse", verificationCode, true);
                    for (int i = 1; i < 11; i++)
                    {
                        status = await Utilities.CheckDurableStatus(durableInstance.statusQueryGetUri, true, i);
                        Thread.Sleep(1000);
                        if (status.runtimeStatus == "Completed")
                        {
                            Utilities.DisplayMessage($"    Verification complete!", ConsoleColor.Green);
                            break;
                        }
                    }
                    if (status.runtimeStatus != "Completed")
                    {
                        Utilities.DisplayMessage($"    Verification process failed!", ConsoleColor.Red);
                    }
                }
                Utilities.DisplayCompletionMessage(timer);
                return true;
            }
            Utilities.DisplayCompletionMessage(timer);
            return false;
        }
        catch (Exception ex)
        {
            Utilities.DisplayErrorMessage($"Error running Interaction Test: {Utilities.GetExceptionMessage(ex)}");
            Utilities.DisplayCompletionMessage(timer);
            return false;
        }
    }
}
