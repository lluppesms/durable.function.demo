namespace Durable.Demo.Sequential;
using Durable.Demo.Sequential.Data;

public static class GetData
{
    [FunctionName("Sequential_GetWorkStepData")]
    public static List<WorkStep> Run([ActivityTrigger] string name)
    {
        //var tasks = JsonConvert.DeserializeObject<List<WorkStep>>(File.ReadAllText("TaskList.json"));
        var tasks = new List<WorkStep>() {
            new WorkStep("Task 1"),
            new WorkStep("Task 2"),
            new WorkStep("Task 3"),
            new WorkStep("Task 4"),
            new WorkStep("Task 5"),
            new WorkStep("Task 6"),
            new WorkStep("Task 7"),
            new WorkStep("Task 8"),
            new WorkStep("Task 9"),
            new WorkStep("Task 10"),
            new WorkStep("Task 11"),
            new WorkStep("Task 12"),
            new WorkStep("Task 13"),
            new WorkStep("Task 14"),
            new WorkStep("Task 15")
        };
        return tasks;
    }
}
