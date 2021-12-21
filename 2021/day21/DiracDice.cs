int PlayUsingDeterministicDie(int[] positions)
{
    positions = positions.ToArray();

    int[] scores = new[] { 0, 0 };
    int player = 0;
    int turn = 0;

    int die = 0;
    int RollDie() => die == 100 ? (die = 1) : ++die;

    while (scores.All(x => x < 1000))
    {
        turn++;

        int roll = RollDie() + RollDie() + RollDie();

        positions[player] = (positions[player] - 1 + roll) % 10 + 1;
        scores[player] += positions[player];

        player = 1 - player;
    }

    return scores.Where(x => x < 1000).First() * turn * 3;
}

Dictionary<(int, int, int, int, int), (long, long)> memoization = new();

(long p1, long p2) PlayUsingQuantumDie(int[] positions, int[] scores, int player)
{
    if (scores.Any(x => x >= 21))
    {
        return (scores[0] >= 21 ? 1 : 0, scores[1] >= 21 ? 1 : 0);
    }

    (int, int, int, int, int) cacheKey = (positions[0], positions[1], scores[0], scores[1], player);
    if (memoization.ContainsKey(cacheKey))
    {
        return memoization[cacheKey];
    }

    List<(long p1, long p2)> wins = new();

    for (int d1 = 1; d1 <= 3; d1++)
    for (int d2 = 1; d2 <= 3; d2++)
    for (int d3 = 1; d3 <= 3; d3++)
    {
        int roll = d1 + d2 + d3;

        int previousPosition = positions[player];
        int previousScore = scores[player];

        positions[player] = (positions[player] - 1 + roll) % 10 + 1;
        scores[player] += positions[player];
        wins.Add(PlayUsingQuantumDie(positions, scores, 1 - player));

        positions[player] = previousPosition;
        scores[player] = previousScore;
    }

    (long, long) result = (wins.Select(x => x.p1).Sum(), wins.Select(x => x.p2).Sum());

    memoization.Add(cacheKey, result);
    return result;
}

int[] STARTING_POSITIONS = new[] { 8, 10 };

Console.WriteLine(PlayUsingDeterministicDie(STARTING_POSITIONS));
Console.WriteLine(PlayUsingQuantumDie(STARTING_POSITIONS, new[] { 0, 0 }, 0));
