char[][] map = File.ReadAllLines("input")
    .Select(x => x.ToCharArray())
    .ToArray();

var DIRECTIONS = new List<(int x, int y)> {
    (0, 1), (0, -1), (1, 0), (-1, 0)
}.AsReadOnly();

IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
{
    foreach (var direction in DIRECTIONS)
    {
        (int x, int y) next = (x + direction.x, y + direction.y);
        if (next.x < 0 || next.x >= map.Length || next.y < 0 || next.y >= map[next.x].Length) continue;
        if (map[next.x][next.y] == 'E' && map[x][y] != 'z') continue;
        if (map[x][y] != 'S' && map[next.x][next.y] != 'E' && map[next.x][next.y] > map[x][y] + 1) continue;
        yield return next;
    }
}

IEnumerable<(int x, int y)> FindCells(char c) => map
    .SelectMany((_, x) => map[x].Select((_, y) => (x, y)))
    .Where(p => map[p.x][p.y] == c);

int PathfindDistanceFromTo(char startCell, char endCell)
{
    var starts = FindCells(startCell);

    HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
    List<(int x, int y, int d)> unvisited = starts.Select(p => (p.x, p.y, 0)).ToList();

    while (unvisited.Count > 0)
    {
        unvisited.Sort((a, b) => a.d - b.d);
        var current = unvisited[0];

        unvisited.RemoveAt(0);
        visited.Add((current.x, current.y));

        if (map[current.x][current.y] == endCell)
        {
            return current.d;
        }

        foreach (var neighbor in GetNeighbors(current.x, current.y))
        {
            if (visited.Contains((neighbor.x, neighbor.y))) continue;
            if (unvisited.Any(p => p.x == neighbor.x && p.y == neighbor.y)) continue;
            unvisited.Add((neighbor.x, neighbor.y, current.d + 1));
        }
    }

    throw new Exception("no path found");
}

Console.WriteLine(PathfindDistanceFromTo('S', 'E'));
Console.WriteLine(PathfindDistanceFromTo('a', 'E'));
