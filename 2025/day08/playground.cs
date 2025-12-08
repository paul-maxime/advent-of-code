List<(long x, long y, long z)> points = File.ReadAllLines("input")
    .Select(x => x.Split(",").Select(long.Parse).ToArray())
    .Select(line => (line[0], line[1], line[2]))
    .ToList();

double Distance((long x, long y, long z) p1, (long x, long y, long z) p2)
{
    return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2) + Math.Pow(p1.z - p2.z, 2));
}

long Compare((long x, long y, long z) p1, (long x, long y, long z) p2)
{
    if (p1.x != p2.x) return p1.x - p2.x;
    if (p1.y != p2.y) return p1.y - p2.y;
    return p1.z - p2.z;
}

var connections = points
    .SelectMany(p1 => points.Where(p2 => Compare(p1, p2) < 0)
    .Select(p2 => (p1, p2, distance: Distance(p1, p2))))
    .OrderBy(r => r.distance)
    .ToList();

long MakeCircuits(int limit)
{
    Dictionary<(long x, long y, long z), int> circuits = [];
    int currentCircuit = 1;

    foreach (var (p1, p2, _) in limit == -1 ? connections : connections.Take(limit))
    {
        int circuit1 = circuits.GetValueOrDefault(p1, 0);
        int circuit2 = circuits.GetValueOrDefault(p2, 0);

        if (circuit1 == 0 && circuit2 == 0)
        {
            circuits[p1] = currentCircuit;
            circuits[p2] = currentCircuit;
            currentCircuit++;
        }
        else if (circuit1 == 0 && circuit2 != 0)
        {
            circuits[p1] = circuits[p2];
        }
        else if (circuit1 != 0 && circuit2 == 0)
        {
            circuits[p2] = circuits[p1];
        }
        else if (circuit1 != 0 && circuit2 != 0 && circuit1 != circuit2)
        {
            foreach (var p in circuits.Where(c => c.Value == circuit2).Select(x => x.Key))
            {
                circuits[p] = circuit1;
            }
        }

        if (limit == -1 && circuits.Count == points.Count && circuits.All(x => x.Value == circuit1))
        {
            return p1.x * p2.x;
        }
    }

    return circuits.Select(x => x.Value)
        .GroupBy(x => x)
        .Select(x => x.Count())
        .OrderDescending()
        .Take(3)
        .Aggregate((a, b) => a * b);
}

Console.WriteLine(MakeCircuits(1000));
Console.WriteLine(MakeCircuits(-1));
