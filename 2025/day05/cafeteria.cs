string[][] input = File.ReadAllText("input")
    .Split("\n\n", StringSplitOptions.TrimEntries)
    .Select(line => line.Split("\n"))
    .ToArray();

List<(long from, long to)> ranges = input[0]
    .Select(line => line.Split("-"))
    .Select(line => (long.Parse(line[0]), long.Parse(line[1])))
    .ToList();

List<long> ingredients = input[1]
    .Select(long.Parse)
    .ToList();

long CountFreshIngredients()
{
    return ingredients.Where(x => ranges.Any(r => x >= r.from && x < r.to)).Count();
}

long ComputeAllIngredients()
{
    List<(long from, long to)> counted = [];
    Queue<(long from, long to)> remaining = new(ranges);
    while (remaining.Count > 0)
    {
        (long from, long to) r1 = remaining.Dequeue();
        bool add = true;
        foreach (var r2 in counted)
        {
            if (r1.to < r2.from || r2.to < r1.from) continue;
            // Conflict, merge ranges and add back to the queue.
            counted.Remove(r2);
            long min = Math.Min(r1.from, r2.from);
            long max = Math.Max(r1.to, r2.to);
            remaining.Enqueue((min, max));
            add = false;
            break;
        }
        if (add) counted.Add(r1);
    }
    return counted.Select(x => x.to - x.from + 1).Sum();
}

Console.WriteLine(CountFreshIngredients());
Console.WriteLine(ComputeAllIngredients());
