string[] map = File.ReadAllLines("input");

int mapHeight = map.Length;
int mapWidth = map[0].Length;

List<(int x, int y)> DIRECTIONS = [(0, 1), (1, 0), (0, -1), (-1, 0)];

long Pathfind(bool isUltra)
{
    PriorityQueue<(int x, int y, int cost, int straight, int prevDx, int prevDy), int> queue = new();
    queue.Enqueue((0, 0, 0, 0, 0, 0), 0);

    HashSet<(int x, int y, int straight, int prevDx, int prevDy)> visited = [];

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        if (!visited.Add((current.x, current.y, current.straight, current.prevDx, current.prevDy)))
        {
            continue;
        }

        if (current.x == mapWidth - 1 && current.y == mapHeight - 1 && (!isUltra || current.straight >= 3))
        {
            return current.cost;
        }

        foreach (var direction in DIRECTIONS)
        {
            if (direction.x == -current.prevDx && direction.y == -current.prevDy) continue;

            var next = (x: current.x + direction.x, y: current.y + direction.y);
            if (next.x < 0 || next.y < 0 || next.x >= mapWidth || next.y >= mapHeight) continue;

            bool isStart = current.prevDx == 0 && current.prevDy == 0;
            bool isNewDirection = direction != (current.prevDx, current.prevDy);

            int newCost = current.cost + (map[next.y][next.x] - '0');
            int newStraight = isNewDirection ? 0 : current.straight + 1;

            if (isUltra && !isStart && isNewDirection && (current.straight + 1) < 4) continue;
            if (newStraight >= (isUltra ? 10 : 3)) continue;

            queue.Enqueue((next.x, next.y, newCost, newStraight, direction.x, direction.y), newCost);
        }
    }

    throw new Exception("no path??");
}

Console.WriteLine(Pathfind(false));
Console.WriteLine(Pathfind(true));
