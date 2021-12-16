int[][] map = File.ReadAllLines("input")
    .Select(x => x.ToCharArray().Select(c => c - '0').ToArray())
    .ToArray();

List<(int x, int y)> directions = new List<(int x, int y)>
{
    (-1, 0), (1, 0), (0, -1), (0, 1)
};

int GetRiskAt(int[][] map, int x, int y)
{
    return (map[y % map.Length][x % map[0].Length] + x / map.Length + y / map.Length - 1) % 9 + 1;
}

int FindPathUsingDijkstra(int[][] map, int scale)
{
    PriorityQueue<(int x, int y, int cost), int> open = new();
    HashSet<(int x, int y)> closed = new();

    int mapHeight = map.Length * scale;
    int mapWidth = map[0].Length * scale;

    open.Enqueue((0, 0, 0), 0);
    while (open.Count > 0)
    {
        var current = open.Dequeue();
        if (!closed.Add((current.x, current.y))) continue;

        if (current.x == mapWidth - 1 && current.y == mapHeight - 1)
        {
            return current.cost;
        }

        IEnumerable<(int x, int y)> neighbors = directions
            .Select(p => (p.x + current.x, p.y + current.y));

        IEnumerable<(int x, int y)> openNeighbors = neighbors
            .Where(p => p.x >= 0 && p.y >= 0 && p.x < mapWidth && p.y < mapHeight)
            .Where(p => !closed.Contains((p.x, p.y)));

        foreach ((int x, int y) neighbor in openNeighbors)
        {
            int cost = current.cost + GetRiskAt(map, neighbor.x, neighbor.y);
            open.Enqueue((neighbor.x, neighbor.y, cost), cost);
        }
    }

    throw new Exception("No path found");
}

Console.WriteLine(FindPathUsingDijkstra(map, 1));
Console.WriteLine(FindPathUsingDijkstra(map, 5));
