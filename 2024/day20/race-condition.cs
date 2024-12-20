string[] map = File.ReadAllLines("input");

(int x, int y) FindLetter(char c) => map.Select((line, y) => (x: line.IndexOf(c), y)).Where(p => p.x > -1).First();

bool IsWall((int x, int y) cell) => map[cell.y][cell.x] == '#';

(int x, int y) start = FindLetter('S');
(int x, int y) end = FindLetter('E');

Dictionary<(int x, int y), int> path = [];

IEnumerable<(int x, int y)> GetNeighbors((int x, int y) cell)
{
    yield return (cell.x + 1, cell.y);
    yield return (cell.x - 1, cell.y);
    yield return (cell.x, cell.y + 1);
    yield return (cell.x, cell.y - 1);
}

void GeneratePath()
{
    (int x, int y) current = start;
    int distance = 0;
    path.Add(current, distance++);
    while (current != end)
    {
        foreach ((int x, int y) neighbor in GetNeighbors(current))
        {
            if (!path.ContainsKey(neighbor) && !IsWall(neighbor))
            {
                current = neighbor;
                break;
            }
        }
        path.Add(current, distance++);
    }
}

int ManhattanDistance((int x, int y) cellA, (int x, int y) cellB)
{
    return Math.Abs(cellA.x - cellB.x) + Math.Abs(cellA.y - cellB.y);
}

int CountCheats(int maxDistance, int savingAtLeast)
{
    int count = 0;
    foreach (var (cellA, scoreA) in path)
    {
        foreach (var (cellB, scoreB) in path)
        {
            if (cellA == cellB) continue;
            int distance = ManhattanDistance(cellA, cellB);
            if (distance > maxDistance) continue;
            int saving = scoreB - scoreA - distance;
            if (saving >= savingAtLeast) count++;
        }
    }
    return count;
}

GeneratePath();

Console.WriteLine(CountCheats(2, 100));
Console.WriteLine(CountCheats(20, 100));
