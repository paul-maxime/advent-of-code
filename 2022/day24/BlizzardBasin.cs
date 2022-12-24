string[] input = File.ReadAllLines("input");
List<(int x, int y, int dx, int dy)> winds = input
    .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
    .Select(p => p.c switch {
        '>' => (x: p.x, y: p.y, dx: 1, dy: 0),
        '<' => (p.x, y: p.y, dx: -1, dy: 0),
        '^' => (p.x, y: p.y, dx: 0, dy: -1),
        'v' => (p.x, y: p.y, dx: 0, dy: 1),
        _ => (x: 0, y: 0, dx: 0, dy: 0),
    })
    .Where(p => p.dx + p.dy != 0)
    .ToList();

int mapHeight = input.Length;
int mapWidth = input[0].Length;

int startX = 1;
int startY = 0;
int endX = mapWidth - 2;
int endY = mapHeight - 1;

void MoveWinds()
{
    winds = winds.Select(wind =>  (
        wind.x + wind.dx == 0 ? mapWidth - 2 : wind.x + wind.dx == mapWidth - 1 ? 1 : wind.x + wind.dx,
        wind.y + wind.dy == 0 ? mapHeight - 2 : wind.y + wind.dy == mapHeight - 1 ? 1 : wind.y + wind.dy,
        wind.dx,
        wind.dy
    )).ToList();
}

var DIRECTIONS = new List<(int x, int y)> { (0, 0), (0, 1), (0, -1), (1, 0), (-1, 0) }.AsReadOnly();

IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
{
    foreach (var direction in DIRECTIONS)
    {
        int nx = x + direction.x;
        int ny = y + direction.y;

        if ((nx == startX && ny == startY) || (nx == endX && ny == endY)) yield return (nx, ny);

        if (nx < 1 || nx > mapWidth - 2 || ny < 1 || ny > mapHeight - 2) continue;
        if (winds.Any(w => w.x == nx && w.y == ny)) continue;
        yield return (nx, ny);
    }
}

int minute = 0;
var superpositions = new HashSet<(int x, int y)> { (startX, startY) };

while (!superpositions.Contains((endX, endY)))
{
    minute++;
    MoveWinds();
    superpositions = superpositions.SelectMany(p => GetNeighbors(p.x, p.y)).ToHashSet();
}

Console.WriteLine($"First trip: {minute}");

superpositions = new HashSet<(int x, int y)> { (endX, endY) };
while (!superpositions.Contains((startX, startY)))
{
    minute++;
    MoveWinds();
    superpositions = superpositions.SelectMany(p => GetNeighbors(p.x, p.y)).ToHashSet();
}

Console.WriteLine($"Second trip: {minute}");

superpositions = new HashSet<(int x, int y)> { (startX, startY) };
while (!superpositions.Contains((endX, endY)))
{
    minute++;
    MoveWinds();
    superpositions = superpositions.SelectMany(p => GetNeighbors(p.x, p.y)).ToHashSet();
}

Console.WriteLine($"Third trip: {minute}");

// void DebugPrintMap()
// {
//     for (int y = 0; y < mapHeight; y++)
//     {
//         for (int x = 0; x < mapWidth; x++)
//         {
//             if (superpositions.Contains((x, y))) Console.Write('E');
//             else if (y == 0 || x == 0 || x == mapWidth - 1 || y == mapHeight - 1) Console.Write('#');
//             else if (winds.Any(w => w.x == x && w.y == y)) Console.Write('x');
//             else Console.Write('.');
//         }
//         Console.WriteLine();
//     }
// }
