string[] input = File.ReadAllLines("input");

int mapHeight = input.Length;
int mapWidth = input[0].Length;

var elements = input.SelectMany((line, y) => line.Select((c, x) => (x, y, c)));

HashSet<(int x, int y)> rocks = elements
    .Where(cell => cell.c == '#')
    .Select(cell => (cell.x, cell.y))
    .ToHashSet();

HashSet<(int x, int y)> stones = elements
    .Where(cell => cell.c == 'O')
    .Select(cell => (cell.x, cell.y))
    .ToHashSet();

HashSet<(int x, int y)> FlipStones(HashSet<(int x, int y)> stones, (int x, int y) direction)
{
    bool gravity = true;
    while (gravity)
    {
        gravity = false;
        stones = stones.Select(stone => {
            (int x, int y) next = (stone.x + direction.x, stone.y + direction.y);
            if (next.y >= 0 && next.x >= 0 && next.x < mapWidth && next.y < mapHeight && !rocks.Contains(next) && !stones.Contains(next))
            {
                gravity = true;
                return next;
            }
            return stone;
        }).ToHashSet();
    }
    return stones;
}

long ComputeNorthernLoad(HashSet<(int x, int y)> stones)
{
    stones = FlipStones(stones, (0, -1));
    return stones.Select(stone => input.Length - stone.y).Sum();
}

List<long> ComputeSpinCycle(HashSet<(int x, int y)> stones, int maxCycles)
{
    List<(int x, int y)> directions = [(0, -1), (-1, 0), (0, 1), (1, 0)];
    List<long> results = [];
    for (int step = 0; step < maxCycles; step++)
    {
        foreach (var direction in directions)
        {
            stones = FlipStones(stones, direction);
        }
        results.Add(stones.Select(stone => mapHeight - stone.y).Sum());
    }
    return results;
}

(int start, int size) FindLoop(List<long> values, int minLoopSize, int maxLoopSize)
{
    for (int start = minLoopSize; start < maxLoopSize; start++)
    {
        for (int size = minLoopSize; size < maxLoopSize; size++)
        {
            if (Enumerable.Range(start, size).All(x => values[x] == values[x + size]))
            {
                return (start, size);
            }
        }
    }
    throw new Exception("No loop found :(");
}

Console.WriteLine(ComputeNorthernLoad(stones));
List<long> cycleResults = ComputeSpinCycle(stones, 300);

var loop = FindLoop(cycleResults, 10, 100);
Console.WriteLine("Loop found: " + loop);

Console.WriteLine(cycleResults[(1000000000 - loop.start - 1) % loop.size + loop.start]);
