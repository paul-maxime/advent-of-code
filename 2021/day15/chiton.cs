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
    Dictionary<(int x, int y), Node> open = new();
    HashSet<(int x, int y)> closed = new();

    int mapHeight = map.Length * scale;
    int mapWidth = map[0].Length * scale;

    open.Add((0, 0), new Node { X = 0, Y = 0, Distance = 0 });

    while (open.Count > 0)
    {
        Node current = open.Values.MinBy(x => x.Distance)!;
        open.Remove((current.X, current.Y));
        closed.Add((current.X, current.Y));

        if (current.X == mapWidth - 1 && current.Y == mapHeight - 1)
        {
            return current.Distance;
        }

        IEnumerable<(int x, int y)> neighbors = directions
            .Select(p => (p.x + current.X, p.y + current.Y));

        IEnumerable<(int x, int y)> openNeighbors = neighbors
            .Where(p => p.x >= 0 && p.y >= 0 && p.x < mapWidth && p.y < mapHeight)
            .Where(p => !closed.Contains((p.x, p.y)));

        foreach ((int x, int y) neighbor in openNeighbors)
        {
            int distance = current.Distance + GetRiskAt(map, neighbor.x, neighbor.y);

            if (open.ContainsKey((neighbor.x, neighbor.y)))
            {
                Node next = open[(neighbor.x, neighbor.y)];
                if (distance < next.Distance)
                {
                    next.Distance = distance;
                }
            }
            else
            {
                Node next = new Node { X = neighbor.x, Y = neighbor.y, Distance = distance };
                open.Add((neighbor.x, neighbor.y), next);
            }
        }
    }

    throw new Exception("No path found");
}

Console.WriteLine(FindPathUsingDijkstra(map, 1));
Console.WriteLine(FindPathUsingDijkstra(map, 5));

class Node
{
    public int X;
    public int Y;
    public int Distance;
}
