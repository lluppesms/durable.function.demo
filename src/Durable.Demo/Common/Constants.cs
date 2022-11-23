namespace Durable.Demo;
public static class Constants 
{
    public const string MockPhoneNumber = "515-555-1212";

    public static class DataSource
    {
        public static class Interaction
        {
            public const string Trigger = "Interaction_Trigger";
            public const string Orchestration = "Interaction_Orchestration";
            public const string DoWork = "Interaction_DoWork";
            public const string SendChallenge = "Interaction_SendChallenge";
            public const string GetData = "Interaction_GetData";
        }
        public static class Fanout
        {
            public const string Trigger = "Fanout_Trigger";
            public const string Orchestration = "Fanout_Orchestration";
            public const string Activity = "Fanout_Activity";
            public const string GetData = "Fanout_GetData";
        }
        public static class Sequential
        {
            public const string Trigger = "Sequential_Trigger";
            public const string Orchestration = "Sequential_Orchestration";
            public const string Activity = "Sequential_Activity";
            public const string GetData = "Sequential_GetData";
            public const string DoWork = "Sequential_DoWork";
        }
    }
}