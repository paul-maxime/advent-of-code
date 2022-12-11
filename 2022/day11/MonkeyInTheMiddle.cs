List<Monkey> ReadMonkeys()
{
    var numericalRegex = new System.Text.RegularExpressions.Regex("([0-9]+)");

    return File.ReadAllText("input")
        .Split("\n\n")
        .Select(block => block.Split("\n"))
        .Select(lines => new Monkey {
            items = numericalRegex.Matches(lines[1]).Select(match => long.Parse(match.Value)).ToList(),
            opType = lines[2][23],
            opValue = int.Parse(numericalRegex.Matches(lines[2]).FirstOrDefault()?.Value ?? "0"),
            divisor = int.Parse(numericalRegex.Matches(lines[3]).First().Value),
            nextTrue = int.Parse(numericalRegex.Matches(lines[4]).First().Value),
            nextFalse = int.Parse(numericalRegex.Matches(lines[5]).First().Value)
        }).ToList();
}

long ComputeMonkeyBusiness(int rounds, bool diminishingWorry)
{
    List<Monkey> monkeys = ReadMonkeys();

    long factor = monkeys.Select(x => x.divisor).Aggregate((a, b) => a * b);

    for (int round = 0; round < rounds; round++)
    {
        foreach (var monkey in monkeys)
        {
            foreach (long item in monkey.items)
            {
                long newValue = item;
                long opValue = monkey.opValue == 0 ? item : monkey.opValue;
                if (monkey.opType == '*') newValue *= opValue;
                if (monkey.opType == '+') newValue += opValue;

                if (diminishingWorry) newValue /= 3;
                newValue %= factor;

                int next = (newValue % monkey.divisor) == 0 ? monkey.nextTrue : monkey.nextFalse;
                monkeys[next].items.Add(newValue);

                monkey.inspections += 1;
            }
            monkey.items.Clear();
        }
    }

    return monkeys.Select(x => x.inspections).OrderDescending().Take(2).Aggregate((a, b) => a * b);
}

Console.WriteLine(ComputeMonkeyBusiness(20, true));
Console.WriteLine(ComputeMonkeyBusiness(10000, false));

class Monkey
{
    public List<long> items = new List<long>();
    public char opType;
    public int opValue;
    public int divisor;
    public int nextTrue;
    public int nextFalse;

    public long inspections = 0;
}
