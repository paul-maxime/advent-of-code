List<(int x, int y)> galaxies = File.ReadAllLines("input")
    .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
    .Where(cell => cell.c == '#')
    .Select(cell => (cell.x, cell.y))
    .ToList();

List<(long x, long y)> ExpandUniverse(List<(int x, int y)> galaxies, long size) => galaxies
    .Select(g => (
        g.x + Enumerable.Range(0, g.x).Where(x => !galaxies.Any(other => other.x == x)).Count() * (size - 1),
        g.y + Enumerable.Range(0, g.y).Where(y => !galaxies.Any(other => other.y == y)).Count() * (size - 1)
    )).ToList();

long ComputeAllDistances(List<(long x, long y)> galaxies) => galaxies
    .SelectMany(a => galaxies
        .Where(b => b != a)
        .Select(b => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y)
    )).Sum() / 2;

Console.WriteLine(ComputeAllDistances(ExpandUniverse(galaxies, 2)));
Console.WriteLine(ComputeAllDistances(ExpandUniverse(galaxies, 1000000)));
