string[] map = File.ReadAllLines("input");

int mapHeight = map.Length;
int mapWidth = map[0].Length;

(int x, int y) start = (1, mapHeight - 2);
(int x, int y) end = (mapWidth - 2, 1);

List<(int x, int y)> directions = [(1, 0), (0, 1), (-1, 0), (0, -1)];

IEnumerable<(int x, int y)> TraverseFinalPath(
    Dictionary<(int x, int y, int direction), (int cost, HashSet<(int x, int y, int direction)> cells)> previous,
    (int x, int y, int direction) cell
)
{
    yield return (cell.x, cell.y);
    if (previous.TryGetValue(cell, out var prev))
    {
        foreach (var way in prev.cells)
        {
            foreach (var node in TraverseFinalPath(previous, way))
            {
                yield return node;
            }
        }
    }
}

void Pathfind()
{
    PriorityQueue<(int x, int y, int direction, int cost), int> open = new();
    HashSet<(int x, int y, int direction)> closed = [];
    Dictionary<(int x, int y, int direction), (int cost, HashSet<(int x, int y, int direction)> cells)> previous = [];

    open.Enqueue((start.x, start.y, 0, 0), 0);

    while (open.Count > 0)
    {
        (int x, int y, int direction, int cost) = open.Dequeue();

        if (x == end.x && y == end.y)
        {
            Console.WriteLine(cost);
            Console.WriteLine(TraverseFinalPath(previous, (x, y, direction)).ToHashSet().Count);
            return;
        }

        if (closed.Contains((x, y, direction))) continue;
        closed.Add((x, y, direction));

        for (int newDirection = 0; newDirection < directions.Count; newDirection++)
        {
            var delta = directions[newDirection];

            (int x, int y, int direction) next = (x + delta.x, y + delta.y, newDirection);
            int nextCost = direction == newDirection ? cost + 1 : cost + 1001;

            if (map[next.y][next.x] == '#') continue;

            if (!previous.ContainsKey(next) || previous[next].cost > nextCost)
            {
                previous[next] = (nextCost, []);
            }
            if (previous[next].cost == nextCost)
            {
                previous[next].cells.Add((x, y, direction));
            }

            open.Enqueue((x + delta.x, y + delta.y, newDirection, nextCost), nextCost);
        }
    }

    Console.WriteLine("No path found :(");
}

Pathfind();
