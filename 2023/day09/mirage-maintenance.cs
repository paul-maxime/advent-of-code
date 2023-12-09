List<long[]> input = File.ReadAllLines("input")
    .Select(line => line.Split(" ").Select(long.Parse).ToArray())
    .ToList();

long[] GenerateNextSequence(long[] input) =>
    Enumerable.Range(0, input.Length - 1)
        .Select(i => input[i + 1] - input[i])
        .ToArray();

long ComputeNext(long[] input)
{
    if (input.All(x => x == input[0]))
    {
        return input[^1];
    }
    return input[^1] + ComputeNext(GenerateNextSequence(input));
}

long ComputePrevious(long[] input)
{
    if (input.All(x => x == input[0]))
    {
        return input[0];
    }
    return input[0] - ComputePrevious(GenerateNextSequence(input));
}

Console.WriteLine(input.Select(ComputeNext).Sum());
Console.WriteLine(input.Select(ComputePrevious).Sum());
