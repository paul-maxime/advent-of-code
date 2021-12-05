int[][][] clouds = File.ReadAllLines("input")
    .Select(line => line
        .Split(" -> ")
        .Select(point => point
            .Split(",")
            .Select(int.Parse)
            .ToArray()
        ).ToArray())
    .ToArray();

int GetCloudCollisions(int[][][] clouds, bool diagonals)
{
    var map = new Dictionary<(int, int), int>();

    foreach (int[][] cloud in clouds)
    {
        int x = cloud[0][0];
        int y = cloud[0][1];
        int toX = cloud[1][0];
        int toY = cloud[1][1];

        if (x != toX && y != toY && !diagonals) continue;

        map[(x, y)] = map.GetValueOrDefault((x, y), 0) + 1;

        while (x != toX || y != toY)
        {
            x += Math.Sign(toX - x);
            y += Math.Sign(toY - y);

            map[(x, y)] = map.GetValueOrDefault((x, y), 0) + 1;
        }
    }

    return map.Where(x => x.Value > 1).Count();
}

Console.WriteLine($"Collisions without diagonals: {GetCloudCollisions(clouds, false)}");
Console.WriteLine($"Collisions with diagonals: {GetCloudCollisions(clouds, true)}");
