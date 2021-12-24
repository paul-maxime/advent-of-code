string[] lines = File.ReadAllLines("input");

List<(char type, int x, int y)> amphipods = lines
    .Take(3)
    .Append("  #D#C#B#A#")
    .Append("  #D#B#A#C#")
    .Append(lines[3])
    .SelectMany((line, y) => line
        .Select((c, x) => (c, x))
        .Where(pod => pod.c >= 'A' && pod.c <= 'D')
        .Select(pod => (pod.c, pod.x, y))
    ).ToList();

Console.WriteLine(Solve(amphipods.Where(pod => pod.y != 3 && pod.y != 4).Select(pod => (pod.type, pod.x, pod.y == 5 ? 3 : 2)).ToList()));
Console.WriteLine(Solve(amphipods));

int Solve(List<(char type, int x, int y)> amphipods)
{
    PriorityQueue<(List<(char type, int x, int y)> pods, int cost), int> queue = new();
    HashSet<string> explored = new();

    queue.Enqueue((amphipods, 0), 0);

    while (queue.Count > 0)
    {
        (amphipods, int currentCost) = queue.Dequeue();

        if (!explored.Add(GetHashFromAmphipods(amphipods)))
        {
            continue;
        }

        if (IsSolved(amphipods))
        {
            return currentCost;
        }

        foreach (var amphipod in amphipods)
        {
            foreach (var movement in GetAvailableMovements(amphipods, amphipod))
            {
                int cost = (Math.Abs(amphipod.x - movement.x) + Math.Abs(amphipod.y - movement.y)) * GetCost(amphipod.type);
                var newPods = amphipods.Select(pod => pod == amphipod ? (amphipod.type, movement.x, movement.y) : pod).ToList();
                if (explored.Contains(GetHashFromAmphipods(newPods)))
                {
                    continue;
                }
                queue.Enqueue((newPods, currentCost + cost), currentCost + cost);
            }
        }

    }

    return -1;
}

int GetRoomHeight(List<(char type, int x, int y)> amphipods) => amphipods.Count == 8 ? 2 : 4;

int GetTargetRoom(int type) => (type - 'A') * 2 + 3;

int GetCost(char type) => type switch {
    'A' => 1,
    'B' => 10,
    'C' => 100,
    'D' => 1000,
    _ => throw new Exception("invalid type"),
};

bool IsSolved(List<(char type, int x, int y)> amphipods)
{
    return amphipods.All(pod => pod.y > 1 && pod.x == GetTargetRoom(pod.type));
}

bool CanMoveFromTo(List<(char type, int x, int y)> amphipods, (int x, int y) from, (int x, int y) to)
{
    while (from.x != to.x || from.y != to.y)
    {
        if (from.y > to.y) from.y--;
        else if (from.x != to.x) from.x += Math.Sign(to.x - from.x);
        else from.y++;
        if (amphipods.Any(pod => pod.x == from.x && pod.y == from.y)) return false;
    }
    return true;
}

IEnumerable<(int x, int y)> GetAvailableMovements(List<(char type, int x, int y)> amphipods, (char type, int x, int y) amphipod)
{
    if (amphipod.y != 1) // Move to hallway
    {
        if (!amphipods.All(pod => pod.type != amphipod.type || (pod.y > 1 && pod.x == GetTargetRoom(amphipod.type))))
        {
            for (int x = 1; x <= 11; x++)
            {
                if (x == 3 || x == 5 || x == 7 || x == 9) continue;
                if (!CanMoveFromTo(amphipods, (amphipod.x, amphipod.y), (x, 1))) continue;
                yield return (x, 1);
            }
        }
    }
    else // Move to room
    {
        int x = GetTargetRoom(amphipod.type);

        if (!amphipods.Any(pod => pod.x == x && pod.y > 1 && pod.type != amphipod.type))
        {
            for (int y = 2; y <= 1 + GetRoomHeight(amphipods); y++)
            {
                if (!CanMoveFromTo(amphipods, (amphipod.x, amphipod.y), (x, y))) continue;
                yield return (x, y);
            }
        }
    }
}

string GetHashFromAmphipods(List<(char type, int x, int y)> amphipods)
{
    System.Text.StringBuilder hash = new System.Text.StringBuilder();
    for (int y = 1; y <= 1 + GetRoomHeight(amphipods); y++)
    {
        for (int x = 1; x <= 11; x++)
        {
            int index = amphipods.FindIndex(pod => pod.x == x && pod.y == y);
            if (index == -1)
            {
                hash.Append('.');
            }
            else
            {
                hash.Append(amphipods[index].type);
            }
        }
    }
    return hash.ToString();
}
