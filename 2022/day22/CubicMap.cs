static class CubicMap
{
    const int RIGHT = 0;
    const int DOWN = 1;
    const int LEFT = 2;
    const int UP = 3;

    static Dictionary<(int face, int direction), (int face, int direction)> WARPS = new Dictionary<(int face, int direction), (int face, int direction)>
    {
        { (3, RIGHT), (2, UP) },
        { (2, DOWN),  (3, LEFT) },
        { (2, RIGHT), (5, LEFT) },
        { (5, RIGHT), (2, LEFT) },
        { (6, RIGHT), (5, UP) },
        { (5, DOWN),  (6, LEFT) },
        { (6, LEFT),  (1, DOWN) },
        { (1, UP),    (6, RIGHT) },
        { (4, LEFT),  (1, RIGHT) },
        { (2, UP),    (6, UP) },
        { (4, UP),    (3, RIGHT) },
        { (3, LEFT),  (4, DOWN) },
        { (1, LEFT),  (4, RIGHT) },
        { (6, DOWN),  (2, DOWN) },
    };

    static int GetCubeFace(int x, int y)
    {
        int fx = x / 50;
        int fy = y / 50;
        switch ((fx, fy))
        {
            case (1, 0): return 1;
            case (2, 0): return 2;
            case (1, 1): return 3;
            case (0, 2): return 4;
            case (1, 2): return 5;
            case (0, 3): return 6;
        }
        throw new Exception("Invalid cube face");
    }

    static (int x, int y, int face) ToLocal(int x, int y)
    {
        int face = GetCubeFace(x, y);
        return (x % 50, y % 50, face);
    }

    static (int x, int y) ToGlobal(int x, int y, int face)
    {
        switch (face)
        {
            case 1: return (x + 50, y);
            case 2: return (x + 100, y);
            case 3: return (x + 50, y + 50);
            case 4: return (x, y + 100);
            case 5: return (x + 50, y + 100);
            case 6: return (x, y + 150);
        }
        throw new Exception("Invalid cube face");
    }

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
                int newDirection = direction;

                if (direction == 0) newX += 1;
                if (direction == 1) newY += 1;
                if (direction == 2) newX -= 1;
                if (direction == 3) newY -= 1;

                if (newX < 0 || newX >= mapWidth || newY < 0 || newY >= mapHeight || newX >= map[newY].Length || map[newY][newX] == ' ')
                {
                    var local = ToLocal(x, y);
                    // Console.WriteLine($"Teleportation required, at {x} {y}, direction {direction}, local {local}");

                    var warpTo = WARPS[(local.face, direction)];

                    // Console.WriteLine($"Warping to {warpTo.face}, direction {warpTo.direction}");

                    int transformedX = local.x;
                    int transformedY = local.y;

                    int angle = (warpTo.direction - direction) * 90;

                    if (angle == 90 || angle == 270 || angle == -90 || angle == -270)
                    {
                        (transformedX, transformedY) = (transformedY, transformedX);
                    }
                    else if (angle == 180 || angle == -180)
                    {
                        if (direction == RIGHT || direction == LEFT)
                        {
                            transformedY = 50 - transformedY - 1;
                        }
                        else
                        {
                            throw new Exception($"Unsupported direction {direction} for angle 180");
                        }
                    }
                    else if (angle == 0)
                    {
                        if (direction == UP || direction == DOWN)
                        {
                            transformedY = 50 - transformedY - 1;
                        }
                        else
                        {
                            throw new Exception($"Unsupported direction {direction} for angle 0");
                        }
                    }
                    else
                    {
                        throw new Exception($"Invalid angle {angle}");
                    }

                    var newGlobal = ToGlobal(transformedX, transformedY, warpTo.face);

                    // Console.WriteLine($"Transformed {transformedX} {transformedY}, new global {newGlobal}");

                    newX = newGlobal.x;
                    newY = newGlobal.y;
                    newDirection = warpTo.direction;
                }

                if (map[newY][newX] == '#')
                {
                    break;
                }

                x = newX;
                y = newY;
                direction = newDirection;
            }
            if (step.rotation == "R") direction = (direction + 1) % 4;
            if (step.rotation == "L") direction = (direction + 3) % 4;
        }

        long password = (y + 1) * 1000 + (x + 1) * 4 + direction;
        Console.WriteLine($"Finished at {x} {y} {direction} -> {password}");
    }
}
