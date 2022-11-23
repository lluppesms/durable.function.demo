namespace Durable.Demo.Sequential;

using Durable.Demo.Sequential.Data;

public static class CreateTasks
{
    private static string DataSource = Constants.DataSource.Sequential.Orchestration;

    [FunctionName("Sequential_CreateTasks")]
    public static async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
    {
        log.LogInformation($"{DataSource} was activated!");

        var duration = context.GetInput<int>();
        var outputs = new List<string>();

        // query a data source to get a list of tasks to execute
        var taskList = await context.CallActivityAsync<List<WorkStep>>("Sequential_GetWorkStepData", null);

        // Refine the list of tasks returned from query
        int secondsPerStep = duration / taskList.Count;
        var numberOfTasks = taskList.Count;
        for (int i = 0; i < numberOfTasks; i++)
        {
            taskList[i].stepNum = i + 1;
            taskList[i].secondsToProcess = secondsPerStep;
        }

        // Go execute the tasks
        foreach (var step in taskList)
        {
            log.LogInformation($"{DataSource} starting step {step}!");
            context.SetCustomStatus(new WorkStatus("Running", step, numberOfTasks));
            outputs.Add(await context.CallActivityAsync<string>("Sequential_DoTask", step));
            context.SetCustomStatus(new WorkStatus("Finished", step, numberOfTasks));
        }

        // return the output when workflow is complete
        return outputs;
    }
}
