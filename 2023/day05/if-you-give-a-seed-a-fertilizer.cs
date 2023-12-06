var input = File.ReadAllText("input")
    .Split("\n\n")
    .Select(part => part.Trim().Split("\n"))
    .ToList();

var seeds = input[0][0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToList();

var seedRanges = seeds.Chunk(2).Select(x => (source: x[0], range: x[1])).ToList();

var maps = input.Skip(1)
    .Select(map => map.Skip(1)
        .Select(line => line.Split(" ").Select(long.Parse).ToArray())
        .Select(map => (dest: map[0], source: map[1], range: map[2]))
        .ToList()
    ).ToList();

long FindLowestLocation(List<long> seeds)
{
    foreach (var map in maps)
    {
        seeds = seeds.Select(seed =>
        {
            var mapping = map.FirstOrDefault(m => seed >= m.source && seed < m.source + m.range);
            return seed - mapping.source + mapping.dest;
        }).ToList();
    }
    return seeds.Min();
}

long FindLowestLocationRange(List<(long source, long range)> seedRanges)
{
    foreach (var map in maps)
    {
        var newSeeds = new List<(long source, long range)>();
        foreach (var seed in seedRanges)
        {
            long seedSource = seed.source;
            long seedRange = seed.range;
            while (seedRange > 0)
            {
                var mapping = map.FirstOrDefault(m => seedSource >= m.source && seedSource < m.source + m.range);
                long available = mapping.range > 0 ? Math.Min(seedRange, mapping.range - (seedSource - mapping.source)) : seedRange;
                newSeeds.Add((seedSource - mapping.source + mapping.dest, available));
                seedSource += available;
                seedRange -= available;
            }
        }
        seedRanges = newSeeds;
    }
    return seedRanges.Select(x => x.source).Min();
}

Console.WriteLine(FindLowestLocation(seeds));
Console.WriteLine(FindLowestLocationRange(seedRanges));
