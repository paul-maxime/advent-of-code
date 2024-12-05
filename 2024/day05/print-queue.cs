string[] input = File.ReadAllLines("input");

List<(long before, long after)> orderingRules = input
    .Where(x => x.Contains('|'))
    .Select(line => line.Split('|'))
    .Select(data => (
        before: long.Parse(data[0]),
        after: long.Parse(data[1])
    ))
    .ToList();

List<List<long>> pagesToPrint = input
    .Where(x => x.Contains(','))
    .Select(line => line.Split(','))
    .Select(data => data.Select(item => long.Parse(item)))
    .Select(data => data.ToList())
    .ToList();

// Part 1

bool CanGoAfter(long page, long previous) =>
    !orderingRules.Any(rule => rule.after == previous && rule.before == page);

bool IsUpdateValid(List<long> pages)
{
    List<long> previous = [];
    foreach (long page in pages)
    {
        if (previous.Any(prev => !CanGoAfter(page, prev)))
        {
            return false;
        }
        previous.Add(page);
    }
    return true;
}

List<List<long>> validUpdates = pagesToPrint.Where(x => IsUpdateValid(x)).ToList();

Console.WriteLine(validUpdates.Select(x => x[x.Count / 2]).Sum());

// Part 2

List<List<long>> invalidUpdates = pagesToPrint.Where(x => !IsUpdateValid(x)).ToList();

int ComparePages(long left, long right)
{
    if (orderingRules.Any(rule => rule.before == left && rule.after == right)) return -1;
    if (orderingRules.Any(rule => rule.before == right && rule.after == left)) return 1;
    return 0;
}

invalidUpdates.ForEach(page => page.Sort(ComparePages));

Console.WriteLine(invalidUpdates.Select(x => x[x.Count / 2]).Sum());
