public class DiceRoll
{
    private readonly Dictionary<string, int> diceRoll;

    public DiceRoll(int[] roll)
    {
        diceRoll = new Dictionary<string, int>
        {
            { "ones", roll.Count(x => x == 1) },
            { "twos", roll.Count(x => x == 2) },
            { "threes", roll.Count(x => x == 3) },
            { "fours", roll.Count(x => x == 4) },
            { "fives", roll.Count(x => x == 5) },
            { "sixes", roll.Count(x => x == 6) }
        };
    }

    public DiceRoll(Dictionary<string, int> roll)
    {
        diceRoll = new Dictionary<string, int>(roll);
    }

    public int CalculateScore()
    {
        return CalculateScore(0);
    }

    private int CalculateScore(int score)
    {
        int bestScore = score;

        foreach (var (partialRoll, partialScore) in ScoringRules.Rules)
        {
            foreach (string key in partialRoll.Keys)
            {
                if (partialRoll[key] <= diceRoll[key])
                {
                    var subGivenRoll = new DiceRoll(diceRoll);
                    subGivenRoll.diceRoll[key] -= partialRoll[key];
                    if (subGivenRoll.HasAScore())
                    {
                        int newScore = subGivenRoll.CalculateScore(score + partialScore);
                        bestScore = Math.Max(bestScore, newScore);
                    }
                    else
                    {
                        bestScore = Math.Max(bestScore, score + partialScore);
                    }
                }
            }
        }
        return bestScore;
    }

    private bool HasAScore()
    {
        foreach (var (partialRoll, partialScore) in ScoringRules.Rules)
        {
            foreach (string key in partialRoll.Keys)
            {
                if (partialRoll[key] <= diceRoll[key])
                { return true; }
            }
        }
        return false;
    }

}
