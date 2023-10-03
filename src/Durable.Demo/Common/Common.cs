namespace Durable.Demo;

public static class Common
{
	/// <summary>
	/// Get the entire request as a string URL or body
	/// </summary>
	public static async Task<string> ParseRequestBodyAsync(HttpRequest req)
	{
		var value = await req.ReadAsStringAsync();
		return value;
	}

	/// <summary>
	/// Parse an integer from the request object
	/// </summary>
    public static int ParseIntegerFromRequest(HttpRequest req, string keyName, int defaultValue = 0)
    {
        var value = req.Query[keyName];
        if (string.IsNullOrEmpty(value))
            return defaultValue;
        try
        {
            int result = Int32.Parse(value);
            return result;
        }
        catch (FormatException)
        {
            return defaultValue;
        }
    }

	/// <summary>
	/// Parse an string from the request object URL or body
	/// </summary>
	public static async Task<string> ParseStringFromRequest(HttpRequest req, string keyName, string defaultValue = "")
	{
		string value = req.Query[keyName];
		try
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			value = value ?? data?.name;
			return (string.IsNullOrEmpty(value)) ? defaultValue : value;
		}
		catch (FormatException)
    {
			return defaultValue;
		}
    }

	/// <summary>
	/// Get an environment variable
	/// </summary>
    public static string GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }

	/// <summary>
	/// Combines all the inner exception messages into one string
	/// </summary>
	public static string GetExceptionMessage(Exception ex)
	{
		var message = string.Empty;
		if (ex == null)
		{
			return message;
		}
		if (ex.Message != null)
		{
			message += ex.Message;
		}
		if (ex.InnerException == null)
		{
			return message;
		}
		if (ex.InnerException.Message != null)
		{
			message += " " + ex.InnerException.Message;
		}
		if (ex.InnerException.InnerException == null)
		{
			return message;
		}
		if (ex.InnerException.InnerException.Message != null)
		{
			message += " " + ex.InnerException.InnerException.Message;
		}
		if (ex.InnerException.InnerException.InnerException == null)
		{
			return message;
		}
		if (ex.InnerException.InnerException.InnerException.Message != null)
		{
			message += " " + ex.InnerException.InnerException.InnerException.Message;
		}
		return message;
	}

	/// <summary>
	/// Returns a sanitized connection string suitable for display on admin page
	/// </summary>
	public static string GetSanitizedConnectionString(string connection)
	{
        //// "DeviceConnectionString": "HostName=iothub123.azure-devices.net;DeviceId=test1;SharedAccessKey=Placeholder-E5Z6******=",
        //// "SQLConnectionString": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=Placeholder-myPassword";
		string noKey;
		if (string.IsNullOrEmpty(connection)) return string.Empty;
		var keyPos = connection.IndexOf("key=", StringComparison.OrdinalIgnoreCase);
		if (keyPos > 0)
		{
			noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
			return noKey;
		}
		keyPos = connection.IndexOf("pwd=", StringComparison.OrdinalIgnoreCase);
		if (keyPos > 0)
		{
			noKey = string.Concat(connection.AsSpan(0, keyPos + 4), "...");
			return noKey;
		}
		keyPos = connection.IndexOf("password=", StringComparison.OrdinalIgnoreCase);
		if (keyPos > 0)
		{
			noKey = string.Concat(connection.AsSpan(0, keyPos + 9), "...");
			return noKey;
		}
		return connection;
	}
}