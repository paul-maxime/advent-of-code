string[] input = File.ReadAllLines("input");

string enhancementAlgorithm = input.First();

HashSet<(int x, int y)> pixels = input
    .Skip(2)
    .SelectMany((line, y) => line.Select((c, x) => (c, x, y)).Where(pixel => pixel.c == '#'))
    .Select(pixel => (pixel.x, pixel.y))
    .ToHashSet();

for (int step = 1; step <= 50; step++)
{
    bool isOutsideInfinite = enhancementAlgorithm[0] == '#' && step % 2 == 0;
    pixels = ApplyAlgorithm(pixels, isOutsideInfinite);
    if (step == 2 || step == 50)
    {
        Console.WriteLine($"After {step} steps: {pixels.Count()} pixels");
    }
}

HashSet<(int x, int y)> ApplyAlgorithm(HashSet<(int x, int y)> input, bool isOutsideInfinite)
{
    HashSet<(int, int)> output = new();

    int minX = input.MinBy(p => p.x).x;
    int maxX = input.MaxBy(p => p.x).x;
    int minY = input.MinBy(p => p.y).y;
    int maxY = input.MaxBy(p => p.y).y;

    for (int x = minX - 1; x <= maxX + 1; x++)
    {
        for (int y = minY - 1; y <= maxY + 1; y++)
        {
            int index = ComputeEnhancementIndex(input, (x, y), (minX, maxX, minY, maxY), isOutsideInfinite);
            if (enhancementAlgorithm[index] == '#')
            {
                output.Add((x, y));
            }
        }
    }

    return output;
}

int ComputeEnhancementIndex(HashSet<(int x, int y)> pixels, (int x, int y) at, (int minX, int maxX, int minY, int maxY) box, bool isOutsideInfinite)
{
    int output = 0;
    for (int y = at.y - 1; y <= at.y + 1; y++)
    {
        for (int x = at.x - 1; x <= at.x + 1; x++)
        {
            output <<= 1;

            bool outsideBox = x < box.minX || x > box.maxX || y < box.minY || y > box.maxY;
            if ((outsideBox && isOutsideInfinite) || pixels.Contains((x, y)))
            {
                output |= 1;
            }
        }
    }
    return output;
}
