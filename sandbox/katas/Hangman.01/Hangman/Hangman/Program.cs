using Hangman;

bool anotherGame = true;

while (anotherGame)
{
    anotherGame = false;
    Console.WriteLine("Hello, let's play HANGMAN!");
    Console.WriteLine("Please, give me a word to be the secret.");
    string? secretWord = ReadHelper.ReadLineMaskedInput('*');
    Console.WriteLine("Great, now give me the number of incorrect guesses that will result in losing the game.");
    int numberOfLives = RetryHelper.TryLoop(() => int.Parse(Console.ReadLine()));
    var hangman = new Hangman.Hangman(secretWord, numberOfLives);
    Console.WriteLine("Okay, time to guess!");

    while (hangman.CurrentState == GameStates.InProgress)
    {
        hangman.PlayOneRound();
    }

    Console.WriteLine("Pressing enter will start another game.");
    Console.WriteLine("-------------------------------------------------------------");
    Console.WriteLine("-------------------------------------------------------------");
    var pressedKey = Console.ReadKey(intercept: true);
    anotherGame = pressedKey.Key == ConsoleKey.Enter;
}


