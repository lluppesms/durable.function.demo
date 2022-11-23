namespace Durable.Test.Harness;

public partial class DurableTest
{
    private async Task<bool> Execute_SequentialTest()
    {
        var timer = new Stopwatch();
        timer.Start();
        try
        {
            Utilities.DisplayMessage($"\nStarting Sequential Test...", ConsoleColor.Yellow);
            var url = $"{config.FunctionUrl}/{Constants.FunctionUrlSuffix.Sequential}";
            var durableInstance = await Utilities.StartDurableFunction(url, string.Empty, Constants.TriggerMethod.GET);
            var status = await Utilities.CheckDurableStatus(durableInstance.statusQueryGetUri, true);
            var i = 0;
            while (status.runtimeStatus != "Completed" && i < 100)
            {
                i++;
                Thread.Sleep(1000);
                status = await Utilities.CheckDurableStatus(durableInstance.statusQueryGetUri, true, i);
                if (status.runtimeStatus == "Completed")
                {
                    Utilities.DisplayMessage($"    Process complete!", ConsoleColor.Green);
                    break;
                }
            }
            if (status.runtimeStatus != "Completed")
            {
                Utilities.DisplayMessage($"    Process failed!", ConsoleColor.Red);
            }
            Utilities.DisplayCompletionMessage(timer);
            return true;
        }
        catch (Exception ex)
        {
            Utilities.DisplayErrorMessage($"Error running Sequential Test: {Utilities.GetExceptionMessage(ex)}");
            Utilities.DisplayCompletionMessage(timer);
            return false;
        }
    }
}
