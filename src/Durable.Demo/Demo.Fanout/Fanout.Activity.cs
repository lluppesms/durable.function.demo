namespace Durable.Demo.Fanout;

public static class Activity
{
    private static string DataSource = Constants.DataSource.Fanout.Activity;

    [FunctionName("Fanout_Activity")]
    public static async Task<int> Run([ActivityTrigger] IDurableActivityContext context, ILogger log)
    {
        MyLogger.InitializeLogger(log);
        MyLogger.LogInfo($"{DataSource} was activated!", DataSource);

        await Task.FromResult(true);
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        Data.Customer customer = context.GetInput<Data.Customer>();
        var objRandom = new Random(Guid.NewGuid().GetHashCode());
        var rndCount = objRandom.Next(customer.OrderCount);
        for (int ndx = 0; ndx <= rndCount; ndx++)
        {
            var percent = (int)Math.Round((double)(100 * ndx) / rndCount);
            log.LogInformation($" Fanout.DoWork - Customer {customer.CustomerId} - Item {ndx} of {rndCount} = {percent}%");
        }
        stopWatch.Stop();
        MyLogger.LogInfo($" Fanout.DoWork completed for Customer {customer.CustomerId}: {stopWatch.Elapsed.TotalSeconds} seconds elapsed", DataSource);
        return rndCount;
    }
}
