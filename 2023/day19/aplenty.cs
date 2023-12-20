const string CATEGORIES = "xmas";

System.Text.RegularExpressions.Regex numericalRegex = new("([0-9]+)");

string[] input = File.ReadAllText("input")
    .Split("\n\n")
    .Select(x => x.Trim())
    .ToArray();

Dictionary<string, List<string[]>> workflows = input[0]
    .Split("\n")
    .Select(line => line.Replace("}", ""))
    .Select(line => line.Split("{"))
    .Select(line => (
        name: line[0],
        checks: line[1].Split(",").Select(check => check.Split(':')).ToList()
    )).ToDictionary();

List<long[]> parts = input[1]
    .Split("\n")
    .Select(line => numericalRegex
        .Matches(line)
        .Select(x => long.Parse(x.Value))
        .ToArray()
    ).ToList();

bool ExecuteWorkflow(string name, long[] part)
{
    foreach (string[] instruction in workflows[name])
    {
        string? destination = null;
        if (instruction.Length == 2)
        {
            string[] operands = instruction[0].Split('>', '<');
            bool isGreater = instruction[0].Contains('>');
            long left = part[CATEGORIES.IndexOf(operands[0])];
            long right = long.Parse(operands[1]);
            if (
                isGreater && left > right ||
                !isGreater && left < right
            )
            {
                destination = instruction[1];
            }
        }
        else
        {
            destination = instruction[0];
        }
        if (destination != null)
        {
            if (destination == "A") return true;
            if (destination == "R") return false;
            return ExecuteWorkflow(destination, part);
        }
    }
    throw new Exception("Unfinished workflow");
}

long ComputePossibilities(string name, (long min, long max)[] possibilities)
{
    if (name == "A")
    {
        return possibilities.Select(x => Math.Max(x.max - x.min + 1, 0)).Aggregate((a, b) => a * b);
    }
    if (name == "R")
    {
        return 0;
    }

    long total = 0;
    foreach (string[] instruction in workflows[name])
    {
        if (instruction.Length == 2)
        {
            string[] operands = instruction[0].Split('>', '<');
            bool isGreater = instruction[0].Contains('>');
            long leftIndex = CATEGORIES.IndexOf(operands[0]);
            long right = long.Parse(operands[1]);

            (long min, long max)[] cloned = [.. possibilities];
            if (isGreater)
            {
                cloned[leftIndex].min = Math.Max(cloned[leftIndex].min, right + 1);
                possibilities[leftIndex].max = Math.Min(possibilities[leftIndex].max, right);
            }
            else
            {
                cloned[leftIndex].max = Math.Min(cloned[leftIndex].max, right - 1);
                possibilities[leftIndex].min = Math.Max(possibilities[leftIndex].min, right);
            }
            total += ComputePossibilities(instruction[1], [.. cloned]);
        }
        if (instruction.Length == 1)
        {
            total += ComputePossibilities(instruction[0], [.. possibilities]);
        }
    }
    return total;
}

Console.WriteLine(parts.Where(part => ExecuteWorkflow("in", part)).Select(part => part.Sum()).Sum());
Console.WriteLine(ComputePossibilities("in", [(1, 4000), (1, 4000), (1, 4000), (1, 4000)]));
