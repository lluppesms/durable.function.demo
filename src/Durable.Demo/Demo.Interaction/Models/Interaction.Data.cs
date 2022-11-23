namespace Durable.Demo.Interaction.Data;

public class DataLog
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Text { get; set; }
}