Dictionary<char, (long x, long y)> DIRECTION_TO_VECTOR = new()
{
    {'U', (0, -1)},
    {'D', (0, 1)},
    {'L', (-1, 0)},
    {'R', (1, 0)},
};

string HEX_TO_DIRECTION = "RDLU";

var plan = File.ReadAllLines("input").Select(line => line.Split(" ")).Select(line => (
    direction: line[0][0],
    distance: long.Parse(line[1]),
    direction2: HEX_TO_DIRECTION[int.Parse(line[2].Substring(7, 1))],
    distance2: Convert.ToInt64(line[2].Substring(2, 5), 16)
)).ToList();

void ComputeArea(bool useColor)
{
    Dictionary<(long x, long y), char> cells = [];
    Dictionary<long, List<long>> changeByLine = [];

    (long x, long y) current = (0, 0);

    char lastHorizontal = 'U';
    foreach (var instruction in plan)
    {
        var direction = useColor ? instruction.direction2 : instruction.direction;
        var distance = useColor ? instruction.distance2 : instruction.distance;

        var vector = DIRECTION_TO_VECTOR[direction];
        if (direction == 'U' || direction == 'D') lastHorizontal = direction;
        for (long i = 0; i < distance; i++)
        {
            var newDirection = direction == 'U' || direction == 'R' || (lastHorizontal == 'U' && direction == 'L') || (direction == 'L' && i > 0) ? 'I' : 'O';

            if (i == 0 || i == distance - 1 || direction == 'U' || direction == 'D')
            {
                cells.Add(current, newDirection);

                if (!changeByLine.ContainsKey(current.y)) changeByLine.Add(current.y, []);
                changeByLine[current.y].Add(current.x);
            }

            current = (current.x + vector.x, current.y + vector.y);
        }
    }

    foreach (var l in changeByLine.Values)
    {
        l.Sort();
    }

    Console.WriteLine(cells.Count + " cells precomputed");

    long total = 0;

    long minY = cells.Keys.Select(cell => cell.y).Min();
    long maxY = cells.Keys.Select(cell => cell.y).Max();
    for (long y = minY; y <= maxY; y++)
    {
        bool inside = false;

        long minX = changeByLine[y].First() - 1;
        long maxX = changeByLine[y].Last() + 1;
        for (long x = minX; x <= maxX; x++)
        {
            bool filled = cells.ContainsKey((x, y));
            if (filled)
            {
                if (cells[(x, y)] == 'I') inside = true;
                if (cells[(x, y)] == 'O') inside = false;
            }
            var potentialNext = changeByLine[y].FirstOrDefault(nextX => nextX >= x, long.MinValue);

            long canSkip = 0;
            if (potentialNext > long.MinValue)
            {
                canSkip = potentialNext - x - 1;
            }
            if (canSkip > 0)
            {
                x += canSkip;
                if (inside)
                {
                    total += canSkip + 1;
                }
            }
            else if (filled || inside)
            {
                total += 1;
            }
        }
        if (useColor && y > 0 && y % 10000 == 0)
        {
            Console.WriteLine(y + " / " + maxY);
        }
    }

    Console.WriteLine(total);
}

ComputeArea(false);
ComputeArea(true);
