HashSet<(int x, int y)> elves = File.ReadAllLines("input")
    .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
    .Where(p => p.c == '#')
    .Select(p => (p.x, p.y))
    .ToHashSet();

int currentRound = 1;
while (true)
{
    var movements = new List<(int fromX, int fromY, int toX, int toY)>();

    foreach (var elf in elves)
    {
        var emptyNorth = !elves.Contains((elf.x - 1, elf.y - 1)) && !elves.Contains((elf.x, elf.y - 1)) && !elves.Contains((elf.x + 1, elf.y - 1));
        var emptySouth = !elves.Contains((elf.x - 1, elf.y + 1)) && !elves.Contains((elf.x, elf.y + 1)) && !elves.Contains((elf.x + 1, elf.y + 1));
        var emptyWest = !elves.Contains((elf.x - 1, elf.y - 1)) && !elves.Contains((elf.x - 1, elf.y)) && !elves.Contains((elf.x - 1, elf.y + 1));
        var emptyEast = !elves.Contains((elf.x + 1, elf.y - 1)) && !elves.Contains((elf.x + 1, elf.y)) && !elves.Contains((elf.x + 1, elf.y + 1));

        if (emptyNorth && emptySouth && emptyWest && emptyEast)
        {
            continue;
        }

        for (int i = 0; i < 4; i++)
        {
            int step = (currentRound - 1 + i) % 4;
            if (step == 0 && emptyNorth)
            {
                movements.Add((elf.x, elf.y, elf.x, elf.y - 1));
                break;
            }
            if (step == 1 && emptySouth)
            {
                movements.Add((elf.x, elf.y, elf.x, elf.y + 1));
                break;
            }
            if (step == 2 && emptyWest)
            {
                movements.Add((elf.x, elf.y, elf.x - 1, elf.y));
                break;
            }
            if (step == 3 && emptyEast)
            {
                movements.Add((elf.x, elf.y, elf.x + 1, elf.y));
                break;
            }
        }
    }

    bool hasMoved = false;
    foreach (var movement in movements.GroupBy(m => (m.toX, m.toY)).Where(x => x.Count() == 1).Select(x => x.First()))
    {
        elves.Remove((movement.fromX, movement.fromY));
        elves.Add((movement.toX, movement.toY));
        hasMoved = true;
    }

    if (currentRound == 10)
    {
        int minX = elves.Select(e => e.x).Min();
        int maxX = elves.Select(e => e.x).Max();
        int minY = elves.Select(e => e.y).Min();
        int maxY = elves.Select(e => e.y).Max();
        Console.WriteLine((maxX - minX + 1) * (maxY - minY + 1) - elves.Count);
    }

    if (!hasMoved)
    {
        Console.WriteLine(currentRound);
        break;
    }

    currentRound++;
}
