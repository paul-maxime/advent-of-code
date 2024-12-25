List<string[]> schematics = File.ReadAllText("input")
    .Split("\n\n")
    .Select(block => block.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .ToList();

IEnumerable<int[]> GetSchematics(char type)
{
    foreach (string[] schematic in schematics)
    {
        int[] lengths = new int[schematic[0].Length];
        for (int x = 0; x < schematic[0].Length; x++)
        {
            int len = 0;
            for (int y = 0; y < schematic.Length; y++)
            {
                if (schematic[y][x] == type) len += 1;
                else break;
            }
            lengths[x] = len;
        }
        if (lengths[0] != 0)
        {
            yield return lengths;
        }
    }
}

List<int[]> locks = GetSchematics('#').ToList();
List<int[]> keys = GetSchematics('.').ToList();

int count = 0;
foreach (int[] lockPins in locks)
{
    foreach (int[] keyPins in keys)
    {
        if (Enumerable.Range(0, lockPins.Length).All(i => lockPins[i] <= keyPins[i]))
        {
            count += 1;
        }
    }
}

Console.WriteLine(count);
