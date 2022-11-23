namespace Durable.Test.Harness.Helpers;

public static partial class Utilities
{
    public static async Task<bool> Call_Url(string url)
    {
        try
        {
            Utilities.DisplayMessage($"      Calling {url}", ConsoleColor.Yellow);
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Utilities.DisplayMessage($"        Success!", ConsoleColor.Green);
                var content = await response.Content.ReadAsStringAsync();
                Utilities.DisplayMessage($"          Response: {content}", ConsoleColor.Yellow);
                return true;
            }
            else
            {
                Utilities.DisplayMessage($"\n      Failed! Status: {response.StatusCode}", ConsoleColor.Red);
                return false;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayError(ex);
            return false;
        }
    }

    public static async Task<DurableInteraction> StartDurableFunction(string url, string bodyContent, string method = Constants.TriggerMethod.POST)
    {
        try
        {
            Utilities.DisplayMessage($"  Calling {url}", ConsoleColor.Yellow);
            var client = new HttpClient();
            StringContent content = null;
            HttpResponseMessage response = null;
            if (method == Constants.TriggerMethod.POST)
            {
                if (!string.IsNullOrEmpty(bodyContent))
                {
                    content = new StringContent(bodyContent);
                }
                response = await client.PostAsync(url, content); 
            }
            else 
            { 
                response = await client.GetAsync(url); 
            }
            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    var rawData = await response.Content.ReadAsStringAsync();
                    var durable = JsonConvert.DeserializeObject<DurableInteraction>(rawData);
                    if (durable != null && !string.IsNullOrEmpty(durable.id))
                    {
                        Utilities.DisplayMessage($"    Started!", ConsoleColor.Blue);
                        return durable;
                    }
                    else
                    {
                        Utilities.DisplayErrorMessage($"    Invalid object returned from call! {rawData}");
                    }
                }
                else
                {
                    Utilities.DisplayErrorMessage($"    Invalid call! {response.StatusCode}");
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Utilities.DisplayError(ex);
            return null;
        }
    }

    public static async Task<DurableStatus> CheckDurableStatus(string url, bool showStatus = false, int iteration = 0)
    {
        try
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response != null)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<DurableStatus>(rawData);
                if (showStatus)
                {
                    var iterationText = (iteration > 0) ? $"{iteration}." : "as of";
                    Utilities.DisplayMessage($"    Status {iterationText} {DateTime.Now:hh:mm:ss} = {status.runtimeStatus}", ConsoleColor.Blue);
                    if (status.customStatus != null)
                    {
                        var customStats = JsonConvert.DeserializeObject<CustomStatus>(status.customStatus.ToString());
                        if (customStats != null)
                        {
                            Utilities.DisplayMessage($"    {customStats.message}", ConsoleColor.Blue);
                            Utilities.DisplayMessage($"    {customStats.progress}  {customStats.percentComplete} %", ConsoleColor.Blue);
                        }
                    }
                }
                return status;
            }
            return null;
        }
        catch (Exception ex)
        {
            Utilities.DisplayError(ex);
            return null;
        }
    }

    public static async Task<HttpResponseMessage> SendDurableEvent(string url, string eventName, string eventContents, bool showStatus = false)
    {
        try
        {
            var client = new HttpClient();
            Utilities.DisplayMessage($"Sending {eventName}...", ConsoleColor.Green);
            url = url.Replace("{eventName}", eventName);
            Utilities.DisplayMessage($"  Calling {url}", ConsoleColor.Yellow);
            Utilities.DisplayMessage($"  HttpContent: {eventContents}", ConsoleColor.Yellow);
            var content = new StringContent(eventContents, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (showStatus)
            {
                Utilities.DisplayMessage($"    Sent Data {eventContents}, function returned status: {response.StatusCode}", ConsoleColor.Blue);
            }
            return response;
        }
        catch (Exception ex)
        {
            Utilities.DisplayError(ex);
            return null;
        }
    }
}
