string[] map = File.ReadAllLines("input");

int mapHeight = map.Length;
int mapWidth = map[0].Length;

(int x, int y) start = Enumerable.Range(0, mapWidth)
    .SelectMany(x => Enumerable.Range(0, mapHeight)
        .Select(y => (x, y, c: map[y][x]))
        .Where(cell => cell.c == 'S'))
    .Select(cell => (cell.x, cell.y))
    .First();

char GetMapCell(int x, int y) => map
    [((y % mapHeight) + mapHeight) % mapHeight]
    [((x % mapWidth) + mapWidth) % mapWidth];

long ComputePositions()
{
    HashSet<(int x, int y)> positions = [start];
    List<(int x, int y)> directions = [(0, 1), (1, 0), (-1, 0), (0, -1)];

    for (int i = 0; i < 64; i++)
    {
        HashSet<(int x, int y)> nextPositions = [];

        foreach (var position in positions)
        {
            foreach (var direction in directions)
            {
                (int x, int y) next = (position.x + direction.x, position.y + direction.y);
                if (next.x < 0 || next.y < 0 || next.x >= mapWidth || next.y >= mapHeight) continue;
                if (map[next.y][next.x] == '#') continue;
                nextPositions.Add(next);
            }
        }

        positions = nextPositions;
    }

    return positions.Count;
}

int FindCycleSize(List<(int value, int delta)> values)
{
    // The cycle is always mapWidth/mapHeight, but I didn't know that...
    for (int cycleSize = 1; cycleSize < 200; cycleSize++)
    {
        bool couldBeValid = true;
        for (int i = values.Count - cycleSize * 3; i < values.Count; i += cycleSize)
        {
            int delta1 = values[i - cycleSize * 2].delta - values[i - cycleSize].delta;
            int delta2 = values[i - cycleSize].delta - values[i].delta;
            if (delta1 != delta2)
            {
                couldBeValid = false;
                break;
            }
        }
        if (couldBeValid)
        {
            return cycleSize;
        }
    }
    throw new Exception("No cycle found...");
}

long ComputeInfinitePositions()
{
    List<(int value, int delta)> values = [];

    HashSet<(int x, int y)> positions = [start];
    List<(int x, int y)> directions = [(0, 1), (1, 0), (-1, 0), (0, -1)];

    int previous = 1;
    for (int i = 0; i < 1000; i++)
    {
        HashSet<(int x, int y)> nextPositions = [];

        foreach (var position in positions)
        {
            foreach (var direction in directions)
            {
                (int x, int y) next = (position.x + direction.x, position.y + direction.y);
                if (GetMapCell(next.x, next.y) == '#') continue;
                nextPositions.Add(next);
            }
        }

        positions = nextPositions;

        values.Add((positions.Count, positions.Count - previous));
        Console.WriteLine((i + 1) + " -> " + positions.Count + " (delta " + (positions.Count - previous) + ")");
        previous = positions.Count;
    }

    int cycleSize = FindCycleSize(values);

    Dictionary<int, long> deltas = [];
    Dictionary<int, long> increments = [];

    for (int i = 0; i <= cycleSize; i++)
    {
        deltas.Add(i + 500, values[i + 500].delta);
        increments.Add(i + 500, values[i + 500].delta - values[i + 500 - cycleSize].delta);
    }

    int index = 500 + cycleSize;
    long value = values[index].value;

    while (index < 26501365 - 1)
    {
        index += 1;
        increments.Add(index, increments[index - cycleSize]);
        deltas.Add(index, deltas[index - cycleSize] + increments[index]);
        value += deltas[index];

        if (index % 100000 == 0) Console.WriteLine((index + 1) + " -> " + value + " (delta " + deltas[index] + ")");
    }

    return value;
}

Console.WriteLine(ComputePositions());
Console.WriteLine(ComputeInfinitePositions());
