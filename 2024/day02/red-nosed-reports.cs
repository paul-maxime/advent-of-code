bool IsSafe(List<long> levels)
{
    bool isIncreasing = Enumerable.Range(0, levels.Count - 1).All(i => levels[i] < levels[i + 1]);
    bool isDecreasing = Enumerable.Range(0, levels.Count - 1).All(i => levels[i] > levels[i + 1]);

    if (!isIncreasing && !isDecreasing) return false;

    long maxDelta = Enumerable.Range(0, levels.Count - 1).Select(i => Math.Abs(levels[i] - levels[i + 1])).Max();
    return maxDelta <= 3;
}

bool IsTolerated(List<long> levels)
{
    return Enumerable.Range(0, levels.Count)
        .Select(ignored => levels.Where((_, i) => i != ignored).ToList())
        .Any(IsSafe);
}

List<List<long>> reports = File.ReadAllLines("input")
    .Select(line => line.Split(" ").Select(long.Parse).ToList())
    .ToList();

Console.WriteLine(reports.Count(IsSafe));
Console.WriteLine(reports.Count(IsTolerated));
