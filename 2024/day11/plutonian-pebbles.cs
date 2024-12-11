List<long> input = File.ReadAllText("input")
    .Trim()
    .Split(" ")
    .Select(long.Parse)
    .ToList();

Dictionary<long, long> Blink(Dictionary<long, long> stones)
{
    Dictionary<long, long> result = [];

    foreach (long stone in stones.Keys)
    {
        long count = stones[stone];
        string stringValue = stone.ToString();

        if (stone == 0)
        {
            result[1] = result.GetValueOrDefault(1) + count;
        }
        else if (stringValue.Length % 2 == 0)
        {
            long left = long.Parse(stringValue.Substring(0, stringValue.Length / 2));
            long right = long.Parse(stringValue.Substring(stringValue.Length / 2, stringValue.Length / 2));
            result[left] = result.GetValueOrDefault(left) + count;
            result[right] = result.GetValueOrDefault(right) + count;
        }
        else
        {
            result[stone * 2024] = result.GetValueOrDefault(stone * 2024) + count;
        }
    }
    return result;
}

void ProcessStones(int times)
{
    Dictionary<long, long> stones = input.ToDictionary(x => x, y => 1L);
    for (int i = 0; i < times; i++)
    {
        stones = Blink(stones);
    }
    Console.WriteLine(stones.Values.Sum());
}

ProcessStones(25);
ProcessStones(75);
