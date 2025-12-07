string[] input = File.ReadAllLines("input");
int startPosition = input[0].IndexOf('S');

long CountSplits()
{
    int splitCount = 0;

    HashSet<int> beams = [startPosition];
    for (int y = 0; y < input.Length - 1; y++)
    {
        HashSet<int> newBeams = [];
        foreach (int x in beams)
        {
            if (input[y + 1][x] == '^')
            {
                splitCount += 1;
                newBeams.Add(x - 1);
                newBeams.Add(x + 1);
            }
            else
            {
                newBeams.Add(x);
            }
        }
        beams = newBeams;
    }

    return splitCount;
}

Dictionary<(int x, int y), long> cachedTimelines = [];
long CountTimelines(int x, int y)
{
    if (y == input.Length - 1) return 1;

    if (cachedTimelines.TryGetValue((x, y), out long cachedValue))
    {
        return cachedValue;
    }

    long value;
    if (input[y + 1][x] == '^')
    {
        value = CountTimelines(x - 1, y + 1) + CountTimelines(x + 1, y + 1);;
    }
    else
    {
        value = CountTimelines(x, y + 1);
    }

    cachedTimelines.Add((x, y), value);
    return value;
}

Console.WriteLine(CountSplits());
Console.WriteLine(CountTimelines(startPosition, 0));
