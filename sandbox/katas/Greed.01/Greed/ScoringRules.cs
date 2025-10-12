public static class ScoringRules
{
    public static readonly (Dictionary<string, int> PartialRoll, int PartialScore)[] Rules =
    {
            (new Dictionary<string, int> {{ "ones", 1 }}, 100),
            (new Dictionary<string, int> {{ "fives", 1 }}, 50),
            (new Dictionary<string, int> {{ "ones", 3 }}, 1000),
            (new Dictionary<string, int> {{ "twos", 3 }}, 200),
            (new Dictionary<string, int> {{ "threes", 3 }}, 300),
            (new Dictionary<string, int> {{ "fours", 3 }}, 400),
            (new Dictionary<string, int> {{ "fives", 3 }}, 500),
            (new Dictionary<string, int> {{ "sixes", 3 }}, 600),
    };
}
