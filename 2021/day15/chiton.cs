int[][] map = File.ReadAllLines("input")
    .Select(x => x.ToCharArray()
    .Select(c => c - '0').ToArray())
    .ToArray();

List<(int x, int y)> directions = new List<(int x, int y)>
{
    (-1, 0), (1, 0), (0, -1), (0, 1)
};

int GetRiskAt(int[][] map, int x, int y)
{
    int risk = map[y % map.Length][x % map[0].Length] + x / map.Length + y / map.Length;
    return risk > 9 ? risk - 9 : risk;
}

int FindPathUsingDijkstra(int[][] map, int scale)
{
    Dictionary<(int x, int y), int> open = new() { { (0, 0), 0 } };
    HashSet<(int x, int y)> closed = new();

    int mapHeight = map.Length * scale;
    int mapWidth = map[0].Length * scale;

    while (open.Count > 0)
    {
        var current = open.MinBy(x => x.Value)!;
        open.Remove((current.Key.x, current.Key.y));
        closed.Add((current.Key.x, current.Key.y));

        if (current.Key.x == mapWidth - 1 && current.Key.y == mapHeight - 1)
        {
            return current.Value;
        }

        IEnumerable<(int x, int y)> neighbors = directions
            .Select(p => (p.x + current.Key.x, p.y + current.Key.y));

        IEnumerable<(int x, int y)> openNeighbors = neighbors
            .Where(p => p.x >= 0 && p.y >= 0 && p.x < mapWidth && p.y < mapHeight)
            .Where(p => !closed.Contains((p.x, p.y)));

        foreach ((int x, int y) neighbor in openNeighbors)
        {
            open[(neighbor.x, neighbor.y)] = Math.Min(
                current.Value + GetRiskAt(map, neighbor.x, neighbor.y),
                open.GetValueOrDefault((neighbor.x, neighbor.y), int.MaxValue)
            );
        }
    }

    throw new Exception("No path found");
}

Console.WriteLine(FindPathUsingDijkstra(map, 1));
Console.WriteLine(FindPathUsingDijkstra(map, 5));
