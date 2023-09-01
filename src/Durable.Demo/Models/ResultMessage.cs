namespace Durable.Demo.Models;

public class ResultMessage
{
	[JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
	public string MethodName { get; set; }

	[JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
	public string? UserName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

	[JsonProperty("recordsRead", NullValueHandling = NullValueHandling.Ignore)]
	public int? RecordsRead { get; set; }

	[JsonProperty("recordsReturned", NullValueHandling = NullValueHandling.Ignore)]
	public int? RecordsReturned { get; set; }

	[JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
	public string Message { get; set; }

	[JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
	public bool? Success { get; set; }

	public ResultMessage()
	{

	}
	public ResultMessage(string message)
	{
		Message = message;
	}
	public ResultMessage(string methodName, string message)
	{
		MethodName = methodName;
		Message = message;
	}
	public ResultMessage(bool success, string message)
	{
		Success = success;
		Message = message;
	}
	public ResultMessage(string methodName, string userName, string message)
	{
		MethodName = methodName;
		UserName = userName;
		Message = message;
	}
	public ResultMessage(string methodName, int recordsReturned, string message)
	{
		MethodName = methodName;
		RecordsReturned = recordsReturned;
		Message = message;
	}
	public ResultMessage(string methodName, string userName,  int recordsReturned, string message)
	{
		MethodName = methodName;
		UserName = userName;
		RecordsReturned = recordsReturned;
		Message = message;
	}
	public ResultMessage(string methodName, string userName, int recordsRead, int recordsReturned, string message)
	{
		MethodName = methodName;
		UserName = userName;
		RecordsRead = recordsRead;
		RecordsReturned = recordsReturned;
		Message = message;
	}
	public ResultMessage(string methodName, string userName, int recordsRead, int recordsReturned, string message, bool success)
	{
		MethodName = methodName;
		UserName = userName;
		RecordsRead = recordsRead;
		RecordsReturned = recordsReturned;
		Message = message;
		Success = success;
	}
}
