string[] lines = File.ReadAllLines("input");

Dictionary<char, char> closingToOpening = new Dictionary<char, char>
{
    { ')', '(' },
    { ']', '[' },
    { '}', '{' },
    { '>', '<' },
};

Dictionary<char, int> pointsPerOperator = new Dictionary<char, int>
{
    { ')', 3 },
    { ']', 57 },
    { '}', 1197 },
    { '>', 25137 },
    { '(', 1 },
    { '[', 2 },
    { '{', 3 },
    { '<', 4 },
};

(int corruption, long incomplete) GetScoreForLine(string line)
{
    Stack<char> operators = new Stack<char>();

    foreach (char c in line)
    {
        if (closingToOpening.ContainsKey(c))
        {
            if (operators.Pop() != closingToOpening[c])
            {
                return (pointsPerOperator[c], 0);
            }
        }
        else
        {
            operators.Push(c);
        }
    }

    long score = 0;
    while (operators.Count > 0)
    {
        score = score * 5 + pointsPerOperator[operators.Pop()];
    }
    return (0, score);
}

(int corruption, long incomplete) GetTotalScores(string[] lines)
{
    int corruptionScore = 0;
    List<long> incompleteScores = new List<long>();

    foreach (string line in lines)
    {
        (int corruption, long incomplete) = GetScoreForLine(line);
        corruptionScore += corruption;
        if (incomplete > 0) incompleteScores.Add(incomplete);
    }
    incompleteScores.Sort();

    return (corruptionScore, incompleteScores[incompleteScores.Count / 2]);
}


Console.WriteLine(GetTotalScores(lines));
