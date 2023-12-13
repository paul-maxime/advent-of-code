List<string[]> patterns = File.ReadAllText("input")
    .Split("\n\n")
    .Select(x => x.Trim().Split("\n").ToArray())
    .ToList();

bool IsReflectionValidX(string[] pattern, int x, bool joker)
{
    int x2 = x + 1;
    while (x >= 0 && x2 < pattern[0].Length)
    {
        int matching = Enumerable.Range(0, pattern.Length).Count(y => pattern[y][x] == pattern[y][x2]);
        if (joker && matching == pattern.Length - 1) joker = false;
        else if (matching != pattern.Length) return false;
        x--;
        x2++;
    }
    return !joker;
}

bool IsReflectionValidY(string[] pattern, int y, bool joker)
{
    int y2 = y + 1;
    while (y >= 0 && y2 < pattern.Length)
    {
        int matching = Enumerable.Range(0, pattern[0].Length).Count(x => pattern[y][x] == pattern[y2][x]);
        if (joker && matching == pattern[0].Length - 1) joker = false;
        else if (matching != pattern[0].Length) return false;
        y--;
        y2++;
    }
    return !joker;
}

int InspectPattern(string[] pattern, bool joker)
{
    IEnumerable<int> mirrorX = Enumerable.Range(0, pattern[0].Length - 1)
        .Where(x => IsReflectionValidX(pattern, x, joker))
        .Select(x => x + 1);

    IEnumerable<int> mirrorY = Enumerable.Range(0, pattern.Length - 1)
        .Where(y => IsReflectionValidY(pattern, y, joker))
        .Select(y => y + 1);

    return mirrorX.FirstOrDefault(0) + mirrorY.FirstOrDefault(0) * 100;
}

Console.WriteLine(patterns.Select(pattern => InspectPattern(pattern, false)).Sum());
Console.WriteLine(patterns.Select(pattern => InspectPattern(pattern, true)).Sum());
