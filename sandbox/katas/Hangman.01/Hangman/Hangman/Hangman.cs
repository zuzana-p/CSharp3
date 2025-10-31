namespace Hangman;

public class Hangman(string secretWord, int numberOfLives)
{
    private readonly string secretWord = secretWord.ToUpperInvariant();
    private readonly int numberOfLives = numberOfLives;
    private readonly List<char>? allGuesses = [];
    private readonly List<char>? wrongGuesses = [];

    private int numberOfIncorrectGuesses;
    private int numberOfCorrectGuesses;
    public GameStates CurrentState { get; set; } = GameStates.InProgress;

    private void CalculateState()
    {
        if (numberOfIncorrectGuesses == numberOfLives)
        {
            CurrentState = GameStates.Lost;
        }
        else if (secretWord.Distinct().Count() == numberOfCorrectGuesses)
        {
            CurrentState = GameStates.Won;
        }
    }

    private GuessResult Guess(char guessedLetter)
    {
        GuessResult result;
        if (char.IsLower(guessedLetter))
        {
            guessedLetter = char.ToUpper(guessedLetter, System.Globalization.CultureInfo.InvariantCulture);
        }

        if (allGuesses.Contains(guessedLetter))
        {
            result = GuessResult.Duplicate;
        }

        else if (secretWord.Contains(guessedLetter))
        {
            numberOfCorrectGuesses += 1;
            result = GuessResult.Correct;
        }
        else
        {
            numberOfIncorrectGuesses += 1;
            wrongGuesses.Add(guessedLetter);
            result = GuessResult.Incorrect;
        }
        allGuesses.Add(guessedLetter);
        CalculateState();
        return result;
    }

    private void PrintMaskedSecretWord(char maskingChar)
    {
        string maskedSecretWord = "";
        foreach (char letter in secretWord)
        {
            if (allGuesses != null && allGuesses.Contains(letter))
            {
                maskedSecretWord += letter;
            }
            else
            {
                maskedSecretWord += maskingChar;
            }
        }
        Console.WriteLine(maskedSecretWord);
    }

    public void PlayOneRound()
    {
        Console.WriteLine();
        Console.WriteLine("-------------------------------------------------------------");
        Console.WriteLine("Please, give me your guess (single letter a-z or A-Z).");
        char guessedLetter = RetryHelper.TryLoop(() => char.Parse(Console.ReadLine()));
        var result = Guess(guessedLetter);
        Console.WriteLine($"The result of your guess is {result}.");
        Console.WriteLine($"You have {numberOfLives - numberOfIncorrectGuesses} lives remaining.");
        if (wrongGuesses.Count != 0)
        {
            Console.WriteLine($"This is the list of your incorrect guesses: {string.Join(' ', wrongGuesses)}.");
        }
        HangmanAsciiArt.DrawHangman(numberOfIncorrectGuesses, numberOfLives);
        PrintMaskedSecretWord('*');

        if (CurrentState == GameStates.Lost)
        {
            HangmanAsciiArt.PrintGameOver();
        }

        if (CurrentState == GameStates.Won)
        {
            HangmanAsciiArt.PrintYouWon();
        }
    }
}

public enum GameStates
{
    Won,
    Lost,
    InProgress
}

public enum GuessResult
{
    Invalid,
    Incorrect,
    Duplicate,
    Correct
}
