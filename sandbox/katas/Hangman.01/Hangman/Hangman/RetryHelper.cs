namespace Hangman;

public static class RetryHelper
{
    public static T TryLoop<T>(Func<T> func) // priznavam, ze tohle je od chatGPT, ale nechala jsem si to vysvetlit :)
    {
        while (true)
        {
            try
            {
                return func();
            }
            catch
            {
                Console.WriteLine("Something went wrong, please try again.");
            }
        }
    }
}
