List<(int x, int y)[]> rocks = File.ReadAllLines("input")
    .Select(line => line
        .Split(" -> ")
        .Select(word => word.Split(","))
        .Select(coords => (int.Parse(coords[0]), int.Parse(coords[1]))
    ).ToArray())
    .ToList();

int maxHeight = rocks.Max(line => line.Max(p => p.y));

bool IsRock(int x, int y) => rocks.Any(lines => Enumerable.Range(0, lines.Length - 1).Any(i =>
    Math.Min(lines[i].x, lines[i + 1].x) <= x && x <= Math.Max(lines[i].x, lines[i + 1].x) &&
    Math.Min(lines[i].y, lines[i + 1].y) <= y && y <= Math.Max(lines[i].y, lines[i + 1].y)
));

bool IsEmpty(HashSet<(int x, int y)> sand, int x, int y) =>
    !IsRock(x, y) && !sand.Contains((x, y)) && !IsBottom(y);

bool PlaceSand(HashSet<(int x, int y)> sand, int x, int y)
{
    if (y > maxHeight) {
        return false;
    }
    if (IsEmpty(sand, x, y + 1)) {
        return PlaceSand(sand, x, y + 1);
    }
    if (IsEmpty(sand, x - 1, y + 1)) {
        return PlaceSand(sand, x - 1, y + 1);
    }
    if (IsEmpty(sand, x + 1, y + 1)) {
        return PlaceSand(sand, x + 1, y + 1);
    }
    sand.Add((x, y));
    return true;
}

int CountSandUntilOutOfBounds()
{
    var sand = new HashSet<(int x, int y)>();
    int total = 0;
    while (PlaceSand(sand, 500, 0))
    {
        total++;
    }
    return total;
}

bool IsBottom(int y) => y == maxHeight + 2;

int CountSand(HashSet<(int x, int y)> sand, int x, int y)
{
    int r = 0;

    if (IsEmpty(sand, x, y + 1)) {
        r += CountSand(sand, x, y + 1);
    }
    if (IsEmpty(sand, x - 1, y + 1)) {
        r += CountSand(sand, x - 1, y + 1);
    }
    if (IsEmpty(sand, x + 1, y + 1)) {
        r += CountSand(sand, x + 1, y + 1);
    }

    sand.Add((x, y));
    return r + 1;
}

int CountSandUntilTop()
{
    var sand = new HashSet<(int x, int y)>();
    return CountSand(sand, 500, 0);
}

Console.WriteLine(CountSandUntilOutOfBounds());
Console.WriteLine(CountSandUntilTop());
