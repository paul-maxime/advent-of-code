List<(int x, int y)> input = File.ReadAllLines("input")
    .Select(line => line.Split(",").Select(int.Parse).ToArray())
    .Select(data => (data[0], data[1]))
    .ToList();

int maxX = input.Select(cell => cell.x).Max();
int maxY = input.Select(cell => cell.y).Max();

bool IsOnMap((int x, int y) cell) => cell.x >= 0 && cell.y >= 0 && cell.x <= maxX && cell.y <= maxY;

IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
{
    yield return (x + 1, y);
    yield return (x - 1, y);
    yield return (x, y + 1);
    yield return (x, y - 1);
}

int Pathfind(int time)
{
    PriorityQueue<(int x, int y, int cost), int> open = new();
    HashSet<(int x, int y)> closed = [.. input.Take(time)];

    open.Enqueue((0, 0, 0), 0);

    while (open.Count > 0)
    {
        (int x, int y, int cost) current = open.Dequeue();

        if (closed.Contains((current.x, current.y))) continue;
        closed.Add((current.x, current.y));

        if (current.x == maxX && current.y == maxY)
        {
            return current.cost;
        }

        var neighbors = GetNeighbors(current.x, current.y)
            .Where(cell => !closed.Contains(cell))
            .Where(IsOnMap)
            .Select(cell => (
                (cell.x, cell.y, current.cost + 1), current.cost + 1
            ));

        open.EnqueueRange(neighbors);
    }

    return -1;
}

Console.WriteLine(Pathfind(1024));

for (int time = 1024; time < input.Count; time++)
{
    if (Pathfind(time) == -1)
    {
        (int x, int y) = input[time - 1];
        Console.WriteLine($"{x},{y}");
        break;
    }
}
