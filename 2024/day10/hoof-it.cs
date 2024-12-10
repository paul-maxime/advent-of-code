string[] map = File.ReadAllLines("input");
int mapHeight = map.Length;
int mapWidth = map[0].Length;

int CellAt(int x, int y)
{
    if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return -1;
    if (map[y][x] == '.') return -1;
    return map[y][x] - '0';
}

int ComputeScoreAt(int x, int y, HashSet<(int x, int y)> exits, bool singlePath)
{
    int cell = CellAt(x, y);
    if (cell == -1) return 0;

    if (cell == 9)
    {
        bool alreadyFound = !exits.Add((x, y));
        return (alreadyFound && singlePath) ? 0 : 1;
    }

    int score = 0;
    if (CellAt(x + 1, y) == cell + 1) score += ComputeScoreAt(x + 1, y, exits, singlePath);
    if (CellAt(x - 1, y) == cell + 1) score += ComputeScoreAt(x - 1, y, exits, singlePath);
    if (CellAt(x, y + 1) == cell + 1) score += ComputeScoreAt(x, y + 1, exits, singlePath);
    if (CellAt(x, y - 1) == cell + 1) score += ComputeScoreAt(x, y - 1, exits, singlePath);
    return score;
}

int ComputeScore(bool singlePath) =>
    Enumerable.Range(0, mapHeight).Select(x => Enumerable.Range(0, mapWidth).Select(y => (x, y)))
        .SelectMany(cell => cell)
        .Where(cell => CellAt(cell.x, cell.y) == 0)
        .Select(cell => ComputeScoreAt(cell.x, cell.y, [], singlePath))
        .Sum();

Console.WriteLine(ComputeScore(true));
Console.WriteLine(ComputeScore(false));
