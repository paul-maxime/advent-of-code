string[] map = File.ReadAllLines("input");

int mapHeight = map.Length;
int mapWidth = map[0].Length;

List<(int x, int y)> DIRECTIONS = [(0, 1), (1, 0), (0, -1), (-1, 0)];

Dictionary<char, (int x, int y)> SLIDES = new() {
    { '>', (1, 0) },
    { 'v', (0, 1) },
};

int PathfindSlowest(int x, int y, HashSet<(int x, int y)> visited, bool hasSlides)
{
    if (y == mapHeight - 1)
    {
        return visited.Count;
    }

    List<(int x, int y)> directions = DIRECTIONS;
    if (hasSlides && SLIDES.TryGetValue(map[y][x], out (int x, int y) value))
    {
        directions = [value];
    }

    int max = 0;
    foreach (var direction in directions)
    {
        int nextX = x + direction.x;
        int nextY = y + direction.y;

        if (nextX < 0 || nextY < 0 || nextX >= mapWidth || nextY >= mapHeight) continue;
        if (map[nextY][nextX] == '#') continue;
        if (visited.Contains((nextX, nextY))) continue;

        max = Math.Max(max, PathfindSlowest(nextX, nextY, visited.Concat([(x, y)]).ToHashSet(), hasSlides));
    }
    return max;
}

int Pathfind((int x, int y) from, (int x, int y) to, HashSet<(int x, int y)> visited)
{
    PriorityQueue<(int x, int y, int cost), int> queue = new();
    queue.Enqueue((from.x, from.y, 0), 0);

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        if (current.x == to.x && current.y == to.y)
        {
            return current.cost;
        }

        if (!visited.Add((current.x, current.y)))
        {
            continue;
        }

        foreach (var direction in DIRECTIONS)
        {
            int nextX = current.x + direction.x;
            int nextY = current.y + direction.y;

            if (nextX < 0 || nextY < 0 || nextX >= mapWidth || nextY >= mapHeight) continue;
            if (map[nextY][nextX] == '#') continue;
            if (visited.Contains((nextX, nextY))) continue;

            queue.Enqueue((nextX, nextY, current.cost + 1), current.cost + 1);
        }
    }

    return -1;
}

IEnumerable<(int x, int y)> FindNodes()
{
    yield return (1, 0);
    for (int x = 1; x < mapWidth - 1; x++)
    {
        for (int y = 1; y < mapHeight - 1; y++)
        {
            if (map[y][x] == '#') continue;
            if (DIRECTIONS.Select(d => (x: x + d.x, y: y + d.y)).Where(next => map[next.y][next.x] != '#').Count() > 2)
            {
                yield return (x, y);
            }
        }
    }
    yield return (mapWidth - 2, mapHeight - 1);
}

Dictionary<(int x, int y), Dictionary<(int x, int y), int>> ComputeGraph(List<(int x, int y)> nodes)
{
    Dictionary<(int x, int y), Dictionary<(int x, int y), int>> distances = [];
    foreach (var nodeA in nodes)
    {
        foreach (var nodeB in nodes)
        {
            int distance = Pathfind(nodeA, nodeB, nodes.Where(x => x != nodeA && x != nodeB).ToHashSet());
            if (distance > 0)
            {
                if (!distances.ContainsKey(nodeA)) distances.Add(nodeA, []);
                distances[nodeA].Add(nodeB, distance);
            }
        }
    }
    return distances;
}

int FindLongestDistance(
    Dictionary<(int x, int y), Dictionary<(int x, int y), int>> graph,
    (int x, int y) from,
    (int x, int y) to,
    HashSet<(int x, int y)> visited,
    int cost
)
{
    if (from == to) return cost;

    int max = 0;
    foreach (var dest in graph[from].Where(x => !visited.Contains(x.Key)))
    {
        max = Math.Max(FindLongestDistance(graph, dest.Key, to, [.. visited, dest.Key], cost + dest.Value), max);
    }
    return max;
}

Console.WriteLine(PathfindSlowest(1, 0, [], true));

// Part 2, too slow with that algorithm.
// Console.WriteLine(PathfindSlowest(1, 0, [], false));

var nodes = FindNodes().ToList();
var graph = ComputeGraph(nodes);
Console.WriteLine(FindLongestDistance(graph, nodes[0], nodes[^1], [nodes[0]], 0));
