// ------------------------------------------------------------------------------------------------------------------------
// WorkActivity: Activity function where the work is done once the user has verified themselves
// ------------------------------------------------------------------------------------------------------------------------
namespace Durable.Demo.Interaction;
public static class WorkActivity
{
    private static string DataSource = Constants.DataSource.Interaction.DoWork;

    [FunctionName("Interaction_DoWork")]
    [return: Table("CustomerTable", Connection = "DataStorageConnectionAppSetting")]
    public static Data.DataLog Run([ActivityTrigger] string phoneNumber, ILogger log)
    {
        MyLogger.InitializeLogger(log);
        MyLogger.LogInfo($"{DataSource} was activated with {phoneNumber}!", DataSource);

        return new Data.DataLog { PartitionKey = $"{DateTime.UtcNow:yyyy-MM-dd}", RowKey = Guid.NewGuid().ToString(), Text = $"Durable-Demo-Interaction: Customer at {phoneNumber} verified an SMS Code!" };
    }
}
