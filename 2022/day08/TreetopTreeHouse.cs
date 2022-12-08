int[][] map = File.ReadAllLines("input")
    .Select(line => line.Select(x => x - '0').ToArray())
    .ToArray();

bool IsVisible(int x, int y)
{
    return IsVisibleLoop(x + 1, y, 1, 0, map[x][y]) ||
        IsVisibleLoop(x - 1, y, -1, 0, map[x][y]) ||
        IsVisibleLoop(x, y + 1, 0, 1, map[x][y]) ||
        IsVisibleLoop(x, y - 1, 0, -1, map[x][y]);
}

bool IsVisibleLoop(int x, int y, int dx, int dy, int max)
{
    if (x < 0 || y < 0 || x >= map.Length || y >= map[0].Length) return true;
    if (map[x][y] >= max) return false;
    return IsVisibleLoop(x + dx, y + dy, dx, dy, max);
}

int ScenicScore(int x, int y)
{
    return ScenicScoreLoop(x + 1, y, 1, 0, map[x][y]) *
        ScenicScoreLoop(x - 1, y, -1, 0, map[x][y]) *
        ScenicScoreLoop(x, y + 1, 0, 1, map[x][y]) *
        ScenicScoreLoop(x, y - 1, 0, -1, map[x][y]);
}

int ScenicScoreLoop(int x, int y, int dx, int dy, int max)
{
    if (x < 0 || y < 0 || x >= map.Length || y >= map[0].Length) return 0;
    if (map[x][y] >= max) return 1;
    return ScenicScoreLoop(x + dx, y + dy, dx, dy, max) + 1;
}

Console.WriteLine(map.Select((line, x) => line.Where((_, y) => IsVisible(x, y)).Count()).Sum());
Console.WriteLine(map.Select((line, x) => line.Select((_, y) => ScenicScore(x, y)).Max()).Max());
