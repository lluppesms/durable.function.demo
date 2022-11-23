namespace Durable.Demo.Fanout;
using Durable.Demo.Fanout.Data;

public static class Orchestrator
{
    private static string DataSource = Constants.DataSource.Fanout.Orchestration;

    [FunctionName("Fanout_Orchestrator")]
    public static async Task<int> Run([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
    {
        var experiment = new Data.Experiment() { CustomerCount = 5, MaxOrderCount = 15000 };
        log.LogInformation($"{DataSource} was activated with {experiment.CustomerCount} customers!");

        var customers = await context.CallActivityAsync<List<Data.Customer>>("Fanout_GetCustomerData", experiment);

        var batchNumber = 0;
        var customersSubmitted = 0;
        var tasks = new List<Task>();
        foreach (var customer in customers)
        {
            customersSubmitted++;
            customer.BatchNumber = batchNumber++;;
            tasks.Add(context.CallActivityAsync<int>("Fanout_Activity", customer));
        }
        
        var totalOrderCount = (from c in customers select c.OrderCount).Sum();
        context.SetCustomStatus(new WorkStatus($"Processing Orders", new WorkStep(0, 0, $"Processing {totalOrderCount} orders..."), customers.Count, "customer"));

        await Task.WhenAll(tasks);
        context.SetCustomStatus(new WorkStatus("Finished", new WorkStep(0, totalOrderCount, $"All {totalOrderCount} Orders for {customers.Count} customers Finished!"), totalOrderCount, "order"));
        log.LogInformation($"{DataSource} finished processing {experiment.CustomerCount} customers!");
        return totalOrderCount;
    }
}
