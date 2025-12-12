string[] input = File.ReadAllText("input").Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

int[] presentSizes = input.SkipLast(1).Select(x => x.Count(c => c == '#')).ToArray();

List<(int width, int height, int[] presents)> regions = input[^1]
    .Split("\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(": "))
    .Select(line => (
        size: line[0].Split("x"),
        presents: line[1].Split(" ")
    ))
    .Select(line => (
        int.Parse(line.size[0]),
        int.Parse(line.size[1]),
        line.presents.Select(int.Parse).ToArray()
    ))
    .ToList();

Console.WriteLine(regions.Count(region =>
{
    int expectedSize = region.presents.Select((x, i) => x * presentSizes[i]).Sum();
    int maxSize = region.width * region.height;
    return expectedSize <= maxSize;
}));
