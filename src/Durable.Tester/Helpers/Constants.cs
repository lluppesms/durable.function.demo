namespace Durable.Test.Harness.Helpers;

public class Constants
{
    /// <summary>
    /// The name of the program
    /// </summary>
    public const string ProgramName = "Durable Functions Tester";

    /// <summary>
    /// Log File Name
    /// </summary>
    public const string LogFileName = "Durable-Test-Harness.log";

    /// <summary>
    /// Get Datified Log File Name
    /// </summary>
    public static string GetLogFileName()
    {
        return Constants.LogFileName.Replace(".log", $"{DateTime.Now.ToString("-yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
    }

    /// <summary>
    /// Environments
    /// </summary>
    public class Environments
    {
        /// <summary>
        /// Development Environment
        /// </summary>
        public const string Dev = "DEV";

        /// <summary>
        /// QA Environment
        /// </summary>
        public const string QA = "QA";

        /// <summary>
        /// Production Environment
        /// </summary>
        public const string Prod = "PROD";
    }

    /// <summary>
    /// Function Types
    /// </summary>
    public static class FunctionType
    {
        /// <summary>
        /// Interaction Function
        /// </summary>
        public const string Interaction = "Interaction";

        /// <summary>
        /// Fanout Function
        /// </summary>
        public const string Fanout = "Fanout";

        /// <summary>
        /// Sequential Function
        /// </summary>
        public const string Sequential = "Sequential";
    }

    /// <summary>
    /// Function URL Suffix
    /// </summary>
    public static class FunctionUrlSuffix
    {
        /// <summary>
        /// Interaction Function
        /// </summary>
        public const string Interaction = "Interaction_Trigger";

        /// <summary>
        /// Fanout Function
        /// </summary>
        public const string Fanout = "Fanout_Trigger?duration=60";

        /// <summary>
        /// Sequential Function
        /// </summary>
        public const string Sequential = "Sequential_Trigger?duration=60";
    }
    
    /// <summary>
    /// Allowed Methods for Trigger Actions
    /// </summary>
    public static class TriggerMethod
    {
        /// <summary>
        /// Post Method
        /// </summary>
        public const string POST = "POST";

        /// <summary>
        /// Get Method
        /// </summary>
        public const string GET = "GET";

    }
}
