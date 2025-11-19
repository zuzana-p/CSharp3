namespace Hangman;

public static class HangmanAsciiArt
{
    private static string hangmanDrawing = @"
+---+
  |   |
  O   |
 /|\  |
 / \  |
      |
=========";

    public static void DrawHangman(int incorrectGuesses, int numberOfLives)
    {
        if (incorrectGuesses == 0)
        {
            return;
        }
        decimal endIndex = (decimal)incorrectGuesses / numberOfLives * hangmanDrawing.Length;
        string reversedDrawing = new([.. hangmanDrawing.Reverse()]);
        string partialDrawing = reversedDrawing.Substring(0, (int)Math.Ceiling(endIndex));
        Console.WriteLine([.. partialDrawing.Reverse()]);
    }

    public static void PrintGameOver() => Console.WriteLine(@"
  ____    _    __  __ _____         _____     _______ ____
 / ___|  / \  |  \/  | ____|       / _ \ \   / / ____|  _ \
| |  _  / _ \ | |\/| |  _|        | | | \ \ / /|  _| | |_) |
| |_| |/ ___ \| |  | | |___       | |_| |\ V / | |___|  _ <
 \____/_/   \_\_|  |_|_____|       \___/  \_/  |_____|_| \_\
 ");

    public static void PrintYouWon() => Console.WriteLine(@"
__   _____  _   _     __        _____  _   _
\ \ / / _ \| | | |    \ \      / / _ \| \ | |
 \ V / | | | | | |     \ \ /\ / / | | |  \| |
  | || |_| | |_| |      \ V  V /| |_| | |\  |
  |_| \___/ \___/        \_/\_/  \___/|_| \_|
  ");
}
