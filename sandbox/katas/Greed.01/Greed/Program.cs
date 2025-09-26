// See https://aka.ms/new-console-template for more information
int[] example1 = [1, 1, 1, 5, 1];
int[] example2 = [2, 3, 4, 6, 2];
int[] example3 = [3, 4, 5, 3, 3];

var givenDiceRoll = new DiceRoll(example3);
Console.WriteLine(givenDiceRoll.CalculateScore());
