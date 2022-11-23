namespace Durable.Test.Harness;

public partial class DurableTest
{
    private ProcessingParameters config = new ProcessingParameters();

    // If you were to do this format, all other "awaits" in the sub functions fail...
    //   you need to start a new thread...
    // public async void Run(ProcessingParameters parms)
    //    await PromptUserForActions(parms);
    public void Run(ProcessingParameters parms)
    {
        config = parms;
        Task.Run(() => PromptUserForActions()).Wait();
        
        // You could change this to run a task directly without prompting the user...
        // Task.Run(() => Execute_UserInteractionTest()).Wait();
    }
}