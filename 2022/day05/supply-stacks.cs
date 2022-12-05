using System.Text.RegularExpressions;

string[] input = File.ReadAllText("input").Split("\n\n");

char[][] initial = input[0].Split("\n")
    .Where(line => line.Contains("["))
    .Select(line => line.Where((x, i) => i % 4 == 1).ToArray())
    .Reverse()
    .ToArray();

Regex numericalRegex = new Regex("([0-9]+)");

List<(int from, int to, int count)> movements = input[1].Split("\n")
    .Select(line => numericalRegex.Matches(line))
    .Where(matches => matches.Count == 3)
    .Select(matches => (int.Parse(matches[1].Value), int.Parse(matches[2].Value), int.Parse(matches[0].Value)))
    .ToList();

List<List<char>> GetInitialStacks() =>
    Enumerable.Range(0, initial[0].Length)
        .Select(y => Enumerable.Range(0, initial.Length)
            .Where(x => y < initial[x].Length && initial[x][y] != ' ')
            .Select(x => initial[x][y])
            .ToList())
        .ToList();

string ApplyMovements9000()
{
    var stacks = GetInitialStacks();
    foreach (var movement in movements)
    {
        for (int i = 0; i < movement.count; i++)
        {
            stacks[movement.to - 1].AddRange(stacks[movement.from - 1].TakeLast(1));
            stacks[movement.from - 1].RemoveAt(stacks[movement.from - 1].Count - 1);
        }
    }
    return string.Join("", stacks.Select(x => x.Last()));
}

string ApplyMovements9001()
{
    var stacks = GetInitialStacks();
    foreach (var movement in movements)
    {
        stacks[movement.to - 1].AddRange(stacks[movement.from - 1].TakeLast(movement.count));
        stacks[movement.from - 1].RemoveRange(stacks[movement.from - 1].Count - movement.count, movement.count);
    }
    return string.Join("", stacks.Select(x => x.Last()));
}

Console.WriteLine(ApplyMovements9000());
Console.WriteLine(ApplyMovements9001());
