static class FlatMap
{
    public static void Process(string[] map, int mapWidth, int mapHeight, List<(int count, string rotation)> path)
    {
        int y = 0;
        int x = map[y].IndexOf(".");
        int direction = 0;

        foreach (var step in path)
        {
            // Console.WriteLine($"Starting at {x} {y} {direction}, executing {step}");
            for (int i = 0; i < step.count; i++)
            {
                int newX = x;
                int newY = y;

                while (true)
                {
                    if (direction == 0) newX += 1;
                    if (direction == 1) newY += 1;
                    if (direction == 2) newX -= 1;
                    if (direction == 3) newY -= 1;

                    newY = (newY + mapHeight) % mapHeight;
                    newX = (newX + mapWidth) % mapWidth;

                    if (newX < map[newY].Length && map[newY][newX] != ' ') break;
                }

                if (map[newY][newX] == '#') break;

                x = newX;
                y = newY;
            }
            if (step.rotation == "R") direction = (direction + 1) % 4;
            if (step.rotation == "L") direction = (direction + 3) % 4;
        }

        long password = (y + 1) * 1000 + (x + 1) * 4 + direction;
        Console.WriteLine($"Finished at {x} {y} {direction} -> {password}");
    }
}
