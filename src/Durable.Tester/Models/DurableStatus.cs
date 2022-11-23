namespace Durable.Test.Harness.Models;

public class DurableStatus
{
    public string name { get; set; }
    public string instanceId { get; set; }
    public string runtimeStatus { get; set; }
    public string input { get; set; }
    public object customStatus { get; set; }
    public object output { get; set; }
    public DateTime createdTime { get; set; }
    public DateTime lastUpdatedTime { get; set; }
}

