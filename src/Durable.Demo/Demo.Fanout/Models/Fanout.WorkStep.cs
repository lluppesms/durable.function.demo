namespace Durable.Demo.Fanout.Data;

public class WorkStep
{
    public int secondsToProcess;
    public int stepNum;
    public string stepName;
    
    public WorkStep(int secondsToProcess, int stepNum, string stepName)
    {
        this.secondsToProcess = secondsToProcess;
        this.stepNum = stepNum;
        this.stepName = stepName;
    }
    public WorkStep(string stepName)
    {
        this.secondsToProcess = 1;
        this.stepNum = 0;
        this.stepName = stepName;
    }
    public WorkStep() {
        this.secondsToProcess = 1;
        this.stepNum = 0;
        this.stepName = string.Empty;
    }
}