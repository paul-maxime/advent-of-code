System.Text.RegularExpressions.Regex numberRegex = new(@"-?[0-9]+");

List<(long x, long y, long vx, long vy)> robots = File.ReadAllLines("input")
    .Select(line => numberRegex.Matches(line)
        .Select(match => long.Parse(match.Value))
        .ToArray()
    )
    .Select(data => (
        x: data[0],
        y: data[1],
        vx: data[2],
        vy: data[3]
    ))
    .ToList();

long mapWidth = robots.Select(robot => robot.x).Max() + 1;
long mapHeight = robots.Select(robot => robot.y).Max() + 1;

static long PositiveModulo(long x, long m) => (x % m + m) % m;

IEnumerable<(long x, long y)> ComputePositionsAfter(int seconds) =>
    robots.Select(robot => (
        x: PositiveModulo(robot.x + robot.vx * seconds, mapWidth),
        y: PositiveModulo(robot.y + robot.vy * seconds, mapHeight)
    ));

IEnumerable<(int x, int y)> ComputeQuadrantsAfter(int seconds) =>
    ComputePositionsAfter(seconds).Select(pos => (
        x: (pos.x < mapWidth / 2) ? -1 : (pos.x > mapWidth / 2) ? 1 : 0,
        y: (pos.y < mapHeight / 2) ? -1 : (pos.y > mapHeight / 2) ? 1 : 0
    ));

int countPerQuadrant = ComputeQuadrantsAfter(100)
    .GroupBy(x => x)
    .Where(group => group.Key.x != 0 && group.Key.y != 0)
    .Select(group => group.Count())
    .Aggregate((a, b) => a * b);

Console.WriteLine(countPerQuadrant);

var bestTree = Enumerable.Range(0, 10000)
    .Select(seconds => (
        index: seconds,
        quadrants: ComputeQuadrantsAfter(seconds)
    ))
    .OrderByDescending(search =>
        Math.Abs(search.quadrants.Where(quad => quad.x == -1).Count() - search.quadrants.Where(quad => quad.x == 1).Count()) *
        Math.Abs(search.quadrants.Where(quad => quad.y == -1).Count() - search.quadrants.Where(quad => quad.y == 1).Count())
    )
    .First();

Console.WriteLine(bestTree.index);

// For debugging
void PrintFrame(List<(long x, long y)> positions)
{
    for (long y = 0; y < mapHeight; y++)
    {
        for (long x = 0; x < mapWidth; x++)
        {
            int count = positions.Count(pos => pos.x == x && pos.y == y);
            if (count == 0) Console.Write('.');
            else Console.Write(count);
        }
        Console.WriteLine();
    }
}

PrintFrame(ComputePositionsAfter(bestTree.index).ToList());
