int[][] octopuses = File.ReadAllLines("input")
    .Select(x => x.ToCharArray().Select(c => c - '0').ToArray())
    .ToArray();

List<(int x, int y)> neighbors = new List<(int, int)>
{
    (-1, -1), (0, -1), (1, -1),
    (-1, 0), (0, 0), (1, 0),
    (-1, 1), (0, 1), (1, 1),
};

int IncrementAndFlash(int[][] octopuses, int x, int y)
{
    if (x < 0 || y < 0 || y >= octopuses.Length || x >= octopuses[y].Length) return 0;
    if (octopuses[y][x] < 0) return 0;

    octopuses[y][x] += 1;

    if (octopuses[y][x] <= 9) return 0;

    int flashes = 1;
    octopuses[y][x] = -1;
    foreach (var direction in neighbors)
    {
        flashes += IncrementAndFlash(octopuses, x + direction.x, y + direction.y);
    }
    return flashes;
}

int SimulateStep(int[][] octopuses)
{
    int flashes = 0;
    for (int y = 0; y < octopuses.Length; y++)
    {
        for (int x = 0; x < octopuses[0].Length; x++)
        {
            flashes += IncrementAndFlash(octopuses, x, y);
        }
    }
    for (int y = 0; y < octopuses.Length; y++)
    {
        for (int x = 0; x < octopuses[0].Length; x++)
        {
            if (octopuses[y][x] < 0) octopuses[y][x] = 0;
        }
    }
    return flashes;
}

int expected = octopuses.Length * octopuses[0].Length;

int step = 0;
int totalFlashes = 0;
int previousFlashes;
do
{
    previousFlashes = SimulateStep(octopuses);
    totalFlashes += previousFlashes;
    step += 1;
    if (step == 100)
    {
        Console.WriteLine(totalFlashes);
    }
} while (previousFlashes != expected);

Console.WriteLine(step);
