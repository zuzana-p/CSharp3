namespace Hangman;

public static class ReadHelper
{
    public static string ReadLineMaskedInput(char maskingChar)
    {
        string input = "";
        ConsoleKeyInfo keyInfo;

        while (true)
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
            }
            else
            {
                input += keyInfo.KeyChar;
                Console.Write(maskingChar);
            }
        }
        return input;
    }
}
