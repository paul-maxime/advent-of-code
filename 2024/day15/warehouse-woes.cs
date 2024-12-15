string[] input = File.ReadAllText("input").Split("\n\n");
string[] map = input[0].Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

int mapHeight = map.Length;
int mapWidth = map[0].Length;

Dictionary<char, (int x, int y)> directions = new () {
    ['^'] = (0, -1),
    ['v'] = (0, 1),
    ['>'] = (1, 0),
    ['<'] = (-1, 0),
};

List<(int x, int y)> movements = input[1]
    .Replace("\n", "")
    .Select(c => directions[c])
    .ToList();

void ParseMap(HashSet<(int x, int y)> walls, HashSet<(int x, int y)> boxes, ref (int x, int y) robot, int scale)
{
    for (int x = 0; x < mapWidth; x++)
    {
        for (int y = 0; y < mapHeight; y++)
        {
            if (map[y][x] == '#') walls.Add((x * scale, y));
            if (map[y][x] == 'O') boxes.Add((x * scale, y));
            if (map[y][x] == '@') robot = (x * scale, y);
        }
    }
}

bool MoveBox(HashSet<(int x, int y)> walls, HashSet<(int x, int y)> boxes, (int x, int y) box, (int x, int y) delta, int scale)
{
    HashSet<(int x, int y)> boxParts = Enumerable.Range(0, scale)
        .Select(i => (box.x + delta.x + i, box.y + delta.y))
        .ToHashSet();

    if (boxParts.Any(part => Enumerable.Range(0, scale).Any(i => walls.Contains((part.x - i, part.y)))))
    {
        // The box collided with a wall.
        return false;
    }

    HashSet<(int x, int y)> collisions = boxParts
        .SelectMany(part => Enumerable.Range(0, scale)
            .Select(i => (part.x - i, part.y))
            .Where(hit => boxes.Contains(hit))
        )
        .Where(hit => hit != box) // don't collide with the same box
        .ToHashSet();

    if (!collisions.All(collision => MoveBox(walls, boxes, collision, delta, scale)))
    {
        // A box pushed by this box collided with a wall.
        return false;
    }

    boxes.Remove(box);
    boxes.Add((box.x + delta.x, box.y + delta.y));
    return true;
}

int SimulateRobot(int scale)
{
    HashSet<(int x, int y)> walls = [];
    HashSet<(int x, int y)> boxes = [];
    (int x, int y) robot = (0, 0);

    ParseMap(walls, boxes, ref robot, scale);

    foreach ((int x, int y) delta in movements)
    {
        (int x, int y) destination = (robot.x + delta.x, robot.y + delta.y);

        if (Enumerable.Range(0, scale).Any(i => walls.Contains((destination.x - i, destination.y))))
        {
            // We hit a wall.
            continue;
        }

        HashSet<(int x, int y)> collisions = Enumerable.Range(0, scale)
            .Select(i => (destination.x - i, destination.y))
            .Where(box => boxes.Contains(box))
            .ToHashSet();

        if (collisions.Count > 0)
        {
            // We can only collide with 1 box, no need to loop.
            (int x, int y) box = collisions.First();
            HashSet<(int x, int y)> newBoxes = [.. boxes];
            if (!MoveBox(walls, newBoxes, box, delta, scale))
            {
                // The box (or a box pushed by this box) collided with a wall.
                continue;
            }
            boxes = newBoxes;
        }

        robot = destination;
    }

    return boxes.Select(box => box.y * 100 + box.x).Sum();
}

Console.WriteLine(SimulateRobot(1));
Console.WriteLine(SimulateRobot(2));
