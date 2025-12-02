bool IsRepeating(long id, long maxSplits)
{
    for (long splits = 2; splits <= maxSplits; splits++)
    {
        if (IsRepeatingInner(id, splits)) return true;
    }
    return false;
}

bool IsRepeatingInner(long id, long splits)
{
    long digits = (long)Math.Floor(Math.Log10(id)) + 1;
    if (digits % splits != 0) return false;

    long divisor = (long)Math.Pow(10, digits / splits);

    long remaining = id;
    long expected = id % divisor;
    while (remaining > 0)
    {
        long current = remaining % divisor;
        if (current != expected) return false;
        remaining /= divisor;
    }

    return true;
}

long SumRepeatingNumbers(long from, long to, long maxSplits)
{
    long total = 0;
    for (long id = from; id < to; id++)
    {
        if (IsRepeating(id, maxSplits))
        {
            total += id;
        }
    }
    return total;
}

List<(long from, long to)> ranges = File.ReadAllText("input")
    .Trim()
    .Split(",")
    .Select(range => range.Split("-"))
    .Select(x => (long.Parse(x[0]), long.Parse(x[1])))
    .ToList();

Console.WriteLine(ranges.Select(x => SumRepeatingNumbers(x.from, x.to, 2)).Sum());
Console.WriteLine(ranges.Select(x => SumRepeatingNumbers(x.from, x.to, 8)).Sum());
