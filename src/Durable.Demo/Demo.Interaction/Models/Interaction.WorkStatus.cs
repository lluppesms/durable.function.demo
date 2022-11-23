namespace Durable.Demo.Interaction.Data;

public class WorkStatus
{
    public string message;
    public int currentStep;
    public int totalSteps;
    public string progress;
    public int percentComplete;

    public WorkStatus(string stateOfStep, WorkStep step, int totalSteps, string objectName = "step")
    {
        this.message = $"{stateOfStep} {objectName} {step.stepNum} of {totalSteps}";
        this.currentStep = step.stepNum;
        this.totalSteps = totalSteps;
        (this.progress, this.percentComplete) = GetProgressBar(step.stepNum, totalSteps);
    }
    
    private static (string, int) GetProgressBar (int stepNum, int numberOfSteps)
    {
        var bars = "";
        var percent = (int)Math.Round((double)(100 * stepNum) / numberOfSteps);
        var barsCompleted = (int)Math.Round((double)percent / 5); // 5% per bar, 20 bars total
        for (int i = 0; i < 20; i++)
        {
            bars += (i < barsCompleted) ? "█" : "░";
        }
        return (bars, percent);
    }
}