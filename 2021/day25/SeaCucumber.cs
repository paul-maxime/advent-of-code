char[][] map = File.ReadAllLines("input")
    .Select(line => line.ToCharArray())
    .ToArray();

bool hasMoved;
int steps = 0;

do
{
    hasMoved = false;
    steps += 1;

    char[][] copy = map.Select(x => x.ToArray()).ToArray();

    for (int x = 0; x < map[0].Length; x++)
    {
        for (int y = 0; y < map.Length; y++)
        {
            int toX = (x + 1) % map[0].Length;
            if (copy[y][x] == '>' && copy[y][toX] == '.')
            {
                hasMoved = true;
                map[y][x] = '.';
                map[y][toX] = '>';
            }
        }
    }

    copy = map.Select(x => x.ToArray()).ToArray();

    for (int x = 0; x < map[0].Length; x++)
    {
        for (int y = 0; y < map.Length; y++)
        {
            int toY = (y + 1) % map.Length;
            if (copy[y][x] == 'v' && copy[toY][x] == '.')
            {
                hasMoved = true;
                map[y][x] = '.';
                map[toY][x] = 'v';
            }
        }
    }
} while (hasMoved);

Console.WriteLine(steps);
