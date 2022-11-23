namespace Durable.Test.Harness.Helpers;

public static partial class Utilities
{
    /// <summary>
    /// Put a date into the middle of a file name
    /// </summary>
    public static string DateifyFileName(string fileName)
    {
        var dateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        var extLocation = fileName.IndexOf(".");
        if (extLocation > 0)
        {
            var fileNameWithDate = fileName[..extLocation] + "-" + dateString + fileName[extLocation..];
            return fileNameWithDate;
        }
        return fileName + dateString;
    }

    /// <summary>
    /// Returns digits - checks to see if string is all numbers, like isnumeric, but works better... commas and periods are ok
    /// </summary>
    public static int ReturnOnlyNumbers(string textToConvert)
    {
        const string Digits = "0123456789";
        var resultString = "0";
        var resultLength = 0;
        try
        {
            int x;
            for (x = 0; x <= textToConvert.Length - 1; x++)
            {
                var lowerCaseChar = textToConvert.Substring(x, 1);
                if (Digits.Contains(lowerCaseChar))
                {
                    resultString += lowerCaseChar;
                    resultLength += 1;
                    if (resultLength > 8)
                    {
                        break;
                    }
                }
            }
            return Convert.ToInt32(resultString);
        }
        catch (Exception ex)
        {
            var message = GetExceptionMessage(ex);
            Console.WriteLine("IsOnlyNumbers: " + message);
            return 9999;
        }
    }

    /// <summary>
    /// Validates that this string has only numbers
    /// </summary>
    public static string IsOnlyLetters(string input)
    {
        const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return IsOnlyTheseCharacters(input, 999, ValidChars);
    }

    /// <summary>
    /// Validates that this string has only number or letters
    /// </summary>
    public static string IsOnlyNumbersOrLetters(string input, int maxLength)
    {
        const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-.";
        return IsOnlyTheseCharacters(input, maxLength, ValidChars);
    }

    /// <summary>
    /// Validates that this string has only allowed characters
    /// </summary>
    public static string IsOnlyTheseCharacters(string input, int maxLength, string validCharacters)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            if (sb.Length < maxLength)
            {
                if (validCharacters.Contains(input[i]))
                {
                    sb.Append(input[i]);
                }
            }
            else
            {
                break;
            }
        }
        var newString = sb.ToString();
        return newString;
    }
}
