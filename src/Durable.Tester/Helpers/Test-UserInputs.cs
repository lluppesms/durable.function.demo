namespace Durable.Test.Harness;

public partial class DurableTest
{
    /// <summary>
    /// Prompt User every few seconds to see what actions they want to take...
    /// </summary>
    public async Task PromptUserForActions()
    {
        bool continueProcessing;
        do
        {
            DisplayTopLevelChoices();
            continueProcessing = await WaitForUserInput();
        }
        while (continueProcessing);
    }

    /// <summary>
    /// Displays the prompt for top level choices
    /// </summary>
    private static void DisplayTopLevelChoices()
    {
        Utilities.DisplayConsoleOnlyMessage(
             "\nDurable Functions Test: \n" +
             "  Press Key: \n" +
             "    I=Interaction Test, F=Fan-Out Test, S=Sequential Test, ESC=Quit", ConsoleColor.Green);
    }

    /// <summary>
    /// Evaluates user input.
    /// </summary>
    private async Task<bool> ProcessKeyboardInputs(bool continueLooping)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Escape:
                    continueLooping = false;
                    break;

                case ConsoleKey.P:
                    Utilities.DisplayConsoleOnlyMessage("\nPaused... Press any key to continue...", ConsoleColor.Magenta);
                    var subkeyP = Console.ReadKey(true).Key;
                    break;

                case ConsoleKey.I:
                case ConsoleKey.U:
                    Utilities.DisplayConsoleOnlyMessage("\n    Executing User Interaction Test...", ConsoleColor.Green);
                    await Execute_UserInteractionTest();
                    break;
                    
                case ConsoleKey.F:
                    Utilities.DisplayConsoleOnlyMessage("\n    Executing Fan Out Test...", ConsoleColor.Green);
                    await Execute_FanOutTest();
                    break;
                    
                case ConsoleKey.S:
                    Utilities.DisplayConsoleOnlyMessage("\n    Executing Sequential Test...", ConsoleColor.Green);
                    await Execute_SequentialTest();
                    break;

                ////case ConsoleKey.LeftArrow:
                ////    ProcessJoystickActions(JoystickKey.Left);
                ////    break;
                ////case ConsoleKey.RightArrow:
                ////    ProcessJoystickActions(JoystickKey.Right);
                ////    break;
                ////case ConsoleKey.UpArrow:
                ////    ProcessJoystickActions(JoystickKey.Up);
                ////    break;
                ////case ConsoleKey.DownArrow:
                ////    ProcessJoystickActions(JoystickKey.Down);
                ////    break;
                ////case ConsoleKey.F1:
                ////    break;
                ////case ConsoleKey.F2:
                ////    break;
                ////case ConsoleKey.F3:
                ////    break;
                ////case ConsoleKey.F4:
                ////    break;
                ////case ConsoleKey.F5:
                ////    break;
                ////case ConsoleKey.F6:
                ////    break;
                ////case ConsoleKey.F7:
                ////    break;
                ////case ConsoleKey.F8:
                ////    break;
                ////case ConsoleKey.F9:
                ////    break;
                ////case ConsoleKey.F10:
                ////    break;
                ////case ConsoleKey.F11:
                ////    break;
                ////case ConsoleKey.F12:
                ////    break;

                ////case ConsoleKey.PageDown:
                ////    break;
                ////case ConsoleKey.PageUp:
                ////    break;
                ////case ConsoleKey.LeftArrow:
                ////    break;
                ////case ConsoleKey.RightArrow:
                ////    break;
                ////case ConsoleKey.UpArrow:
                ////    break;
                ////case ConsoleKey.DownArrow:
                ////    break;
                ////case ConsoleKey.Enter:
                ////    break;
                ////case ConsoleKey.End:
                ////    break;
                ////case ConsoleKey.Tab:
                ////    break;

                default:
                    DisplayTopLevelChoices();
                    break;
            }
        }
        return continueLooping;
    }

    /// <summary>
    /// Waits for user to press a key
    /// </summary>
    private async Task<bool> WaitForUserInput()
    {
        var continueProcessing = true;
        TimeSpan timeLeft;

        var waitTime = DateTime.Now.AddSeconds(10);
        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(250));
            continueProcessing = await ProcessKeyboardInputs(continueProcessing);
            timeLeft = waitTime.Subtract(DateTime.Now);
            if (timeLeft.TotalMilliseconds > 0)
            {
                Console.Write(".", ConsoleColor.White);
            }
        }
        while (timeLeft.TotalMilliseconds > 0 && continueProcessing);
        return continueProcessing;
    }

    private string GetPhoneNumberFromUser()
    {
        var phoneNumber = config.PhoneNumber;
        if (string.IsNullOrEmpty(phoneNumber))
        {
            Utilities.DisplayMessage($"\n{DateTime.Now:hh:mm:ss} Please enter a phone number to send the verification code to:", ConsoleColor.Cyan);
            phoneNumber = GetNumericInput(true);
            phoneNumber = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            Utilities.DisplayMessage(string.Empty);
        }
        return phoneNumber;
    }

    private static string GetVerificationCodeFromUser()
    {
        Utilities.DisplayMessage($"\n{DateTime.Now:hh:mm:ss} Please enter the verification code sent to phone :", ConsoleColor.Cyan);
        var verificationCode = GetNumericInput();
        Utilities.DisplayMessage(string.Empty);
        return verificationCode;
    }

    private static string GetNumericInput(bool allowExtraNumericChars = false)
    {
        var val = 0d;
        var num = string.Empty;
        ConsoleKeyInfo chr;
        do
        {
            chr = Console.ReadKey(true);
            if (chr.Key == ConsoleKey.Enter || chr.Key == ConsoleKey.Escape)
            {
                break;
            }
            if (chr.Key != ConsoleKey.Backspace)
            {
                var control = double.TryParse(chr.KeyChar.ToString(), out val);
                var keyChar = chr.KeyChar;
                if (control || (allowExtraNumericChars && (chr.Key == ConsoleKey.Spacebar || keyChar == '+' || keyChar == '-' || keyChar == '(' || keyChar == ')')))
                {
                    num += chr.KeyChar;
                    Console.Write(chr.KeyChar);
                }
            }
            else
            {
                if (chr.Key == ConsoleKey.Backspace && num.Length > 0)
                {
                    num = num.Substring(0, (num.Length - 1));
                    Console.Write("\b \b");
                }
            }
        }
        while (chr.Key != ConsoleKey.Enter);
        return num;
    }
}
