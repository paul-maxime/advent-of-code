string[] input = File.ReadAllLines("input");
string[] patterns = input[0].Split(", ");
string[] designs = input.Skip(2).ToArray();

Dictionary<(string, int), long> cache = [];

long CountMatches(string design, int index = 0)
{
    if (index == design.Length)
    {
        return 1;
    }
    if (cache.TryGetValue((design, index), out long result))
    {
        return result;
    }

    long total = patterns
        .Where(pattern => design.AsSpan(index).StartsWith(pattern))
        .Select(pattern => CountMatches(design, index + pattern.Length))
        .Sum();

    cache.Add((design, index), total);
    return total;
}

Console.WriteLine(designs.Where(design => CountMatches(design) > 0).Count());
Console.WriteLine(designs.Select(design => CountMatches(design)).Sum());
