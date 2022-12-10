List<(char dir, int num)> movements = File.ReadAllLines("input")
    .Select(line => line.Split(" "))
    .Select(line => (line[0][0], int.Parse(line[1])))
    .ToList();

(int x, int y) head = (0, 0);
List<(int x, int y)> tails = new List<(int x, int y)> { (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0) };
List<HashSet<(int, int)>> visited = tails.Select(pos => new HashSet<(int, int)> { pos }).ToList();

foreach (var movement in movements)
{
    for (int i = 0; i < movement.num; i++)
    {
        switch (movement.dir)
        {
            case 'U':
                head.y -= 1;
                break;
            case 'D':
                head.y += 1;
                break;
            case 'L':
                head.x -= 1;
                break;
            case 'R':
                head.x += 1;
                break;
        }
        (int x, int y) prev = head;
        for (int id = 0; id < tails.Count; id++)
        {
            if (Math.Abs(prev.x - tails[id].x) > 1 || Math.Abs(prev.y - tails[id].y) > 1)
            {
                if (Math.Abs(prev.x - tails[id].x) > 0) tails[id] = (tails[id].x + Math.Sign(prev.x - tails[id].x), tails[id].y);
                if (Math.Abs(prev.y - tails[id].y) > 0) tails[id] = (tails[id].x, tails[id].y + Math.Sign(prev.y - tails[id].y));
            }
            prev = tails[id];
            visited[id].Add(tails[id]);
        }
    }
}

Console.WriteLine(visited[0].Count);
Console.WriteLine(visited[8].Count);
