List<(string springs, long[] damaged)> records = File.ReadAllLines("input")
    .Select(line => line.Split(" "))
    .Select(line => (springs: line[0], damaged: line[1].Split(",").Select(long.Parse).ToArray()))
    .ToList();

List<(string springs, long[] damaged)> unfolded = records
    .Select(record => (
        string.Join("?", Enumerable.Repeat(record.springs, 5)),
        Enumerable.Repeat(record.damaged, 5).SelectMany(x => x).ToArray()
    )).ToList();

static long CountArrangements(
    string springs,
    long[] damaged,
    int springIndex,
    int damageIndex,
    int damagedCount,
    Dictionary<(int, int, int), long> cache
)
{
    if (cache.TryGetValue((springIndex, damageIndex, damagedCount), out long cachedResult))
    {
        return cachedResult;
    }

    if (springIndex == springs.Length)
    {
        if (damageIndex == damaged.Length && damagedCount == 0) return 1;
        if (damageIndex == damaged.Length - 1 && damagedCount == damaged[damageIndex]) return 1;
        return 0;
    }

    char current = springs[springIndex];
    long total = 0;

    if (current == '#' || current == '?')
    {
        if (damageIndex < damaged.Length && damagedCount < damaged[damageIndex])
        {
            total += CountArrangements(springs, damaged, springIndex + 1, damageIndex, damagedCount + 1, cache);
        }
        else if (current == '#') return 0;
    }
    if (current == '.' || current == '?')
    {
        if (damagedCount == 0 || damagedCount == damaged[damageIndex])
        {
            total += CountArrangements(springs, damaged, springIndex + 1, damageIndex + (damagedCount > 0 ? 1 : 0), 0, cache);
        }
        else if (current == '.') return 0;
    }

    cache.Add((springIndex, damageIndex, damagedCount), total);
    return total;
}

Console.WriteLine(records.Select(x => CountArrangements(x.springs, x.damaged, 0, 0, 0, [])).Sum());
Console.WriteLine(unfolded.Select(x => CountArrangements(x.springs, x.damaged, 0, 0, 0, [])).Sum());
