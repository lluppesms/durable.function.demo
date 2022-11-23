namespace Durable.Test.Harness;

public static class Configurator
{
    /// <summary>
    /// Read configuration file based on first command line argument, or return help
    /// </summary>
    public static ProcessingParameters Read(string[] args)
    {
        Utilities.DisplayMessageInABox(Constants.ProgramName, ConsoleColor.Green);

        var configFileName = args.Length > 0 ? args[0] : string.Empty;
        if (configFileName.Contains("help", StringComparison.InvariantCultureIgnoreCase) || configFileName.Contains("?"))
        {
            DisplayHelp();
            return null;
        }
        
        if (string.IsNullOrEmpty(configFileName))
        {
            configFileName = "config.json";
        }
        
        var applicationDirectory = Utilities.GetApplicationDirectory();
        var fullConfigFileName = applicationDirectory + configFileName;

        if (!File.Exists(fullConfigFileName))
        {
            Utilities.DisplayClippyErrorMessage($"Are you sure that's right?  I couldn't find this config file!\n  {fullConfigFileName}");

            //Utilities.DisplayErrorMessage("No config file found...");
            //Utilities.DisplayErrorMessage("Searched for " + configFileName);
            //Utilities.DisplayErrorMessage($"  in directory: {applicationDirectory}");
            //Utilities.DisplayErrorMessage($"  Assembly Directory: {AppContext.BaseDirectory}");
            //Utilities.DisplayErrorMessage($"  Module: {Path.GetFileName(Environment.ProcessPath)}");
            DisplayHelp();
            return null;
        }
        var parms = new ProcessingParameters();
        Utilities.DisplayMessage("Reading config data from " + fullConfigFileName);
        var configRawData = File.ReadAllText(fullConfigFileName);
        var config = JsonConvert.DeserializeObject<Parameters>(configRawData);
        if (config != null)
        {
            parms.EnvironmentCode = ValidateEnvironmentCode(config.EnvironmentCode);
            parms.FunctionUrl = string.IsNullOrEmpty(config.FunctionUrl) ? string.Empty : config.FunctionUrl;
            if (parms.FunctionUrl.EndsWith("/")) { parms.FunctionUrl = parms.FunctionUrl[..^1]; }
            parms.PhoneNumber = config.PhoneNumber;
            
            // put other parameter population here...
        }
        parms.DisplayConfigurationValues();

        Utilities.RemoveOldLogFile();

        return parms;
    }

    /// <summary>
    /// Validate/qualify environment code parameter
    /// </summary>
    public static string ValidateEnvironmentCode(string value)
    {
        var env = string.IsNullOrEmpty(value) ? Constants.Environments.Dev : value.ToUpper().Trim();
        if (env.Contains(Constants.Environments.QA))
        {
            return Constants.Environments.QA;
        }
        if (env.Contains(Constants.Environments.Prod))
        {
            return Constants.Environments.Prod;
        }
        return Constants.Environments.Dev;
    }

    /// <summary>
    /// Display Help Information
    /// </summary>
    public static void DisplayHelp()
    {
        var color = ConsoleColor.Yellow;
        var codeColor = ConsoleColor.Blue;
        Utilities.DisplayMessage("\nThis program expects a command line parameter with a configuration file name, like this:", color);
        Utilities.DisplayMessage("  Durable.Test.Harness.exe config-dev.json", codeColor);
        Utilities.DisplayMessage("\nThe configuration file should contain JSON data similar to this: (not all are required)", color);
        Utilities.DisplayMessage("\n  {", codeColor);
        Utilities.DisplayMessage("    \"EnvironmentCode\":         \"DEV\",", codeColor);
        Utilities.DisplayMessage("    \"FunctionUrl\":             \"<myFunctionUrl>\",", codeColor);
        Utilities.DisplayMessage("    \"PhoneNumber\":             \"+19525551212\",", codeColor);
        Utilities.DisplayMessage("  }", codeColor);
    }
}