string[] input = File.ReadAllLines("input");

int mapHeight = input.Length;
int mapWidth = input[0].Length;

HashSet<(int, int)> obstacles = [];
(int x, int y) guard = (0, 0);

for (int x = 0; x < mapWidth; x++)
{
    for (int y = 0; y < mapHeight; y++)
    {
        if (input[y][x] == '#') obstacles.Add((x, y));
        if (input[y][x] == '^') guard = (x, y);
    }
}

(HashSet<(int, int)> visited, bool loop) GetVisitedCells((int x, int y) guard, (int x, int y) newObstacle)
{
    HashSet<(int, int)> visited = [];
    HashSet<((int, int), (int, int))> visitedWithDirection = [];
    (int x, int y) direction = (0, -1);

    while (guard.x >= 0 && guard.x < mapWidth && guard.y >= 0 && guard.y < mapHeight)
    {
        if (visitedWithDirection.Contains((guard, direction)))
        {
            // Already went that way on that cell, we're in a loop.
            return (visited, true);
        }
        visitedWithDirection.Add((guard, direction));
        visited.Add(guard);

        (int x, int y) front = (guard.x + direction.x, guard.y + direction.y);
        if (obstacles.Contains(front) || front == newObstacle)
        {
            // Rotate direction 90 degrees: sin(90) = 1 ; cos(90) = 0
            direction = (-1 * direction.y, 1 * direction.x);
        }
        else
        {
            guard = front;
        }
    }

    // Went out of the map.
    return (visited, false);
}

HashSet<(int, int)> visited = GetVisitedCells(guard, (-1, -1)).visited;
Console.WriteLine(visited.Count);

// Try to add obstacles everywhere the guard moves. Count loops.
Console.WriteLine(visited.Where(obstacle => GetVisitedCells(guard, obstacle).loop).Count());
