(bool[] indicators, List<int[]> buttons, int[] joltages) ParseMachine(string line)
{
    string[] data = line.Split(" ");
    return (
        indicators: ParseIndicators(data[0]),
        buttons: ParseButtons(data.Skip(1).SkipLast(1)),
        joltages: ParseJoltages(data[^1])
    );
}

bool[] ParseIndicators(string indicators)
{
    return indicators.Skip(1).SkipLast(1).Select(x => x == '#').ToArray();
}

List<int[]> ParseButtons(IEnumerable<string> buttons)
{
    return buttons.Select(x => x.Replace("(", "").Replace(")", "").Split(",").Select(int.Parse).ToArray()).ToList();
}

int[] ParseJoltages(string joltages)
{
    return joltages.Replace("{", "").Replace("}", "").Split(",").Select(int.Parse).ToArray();
}

int SolveIndicators(bool[] targetIndicators, List<int[]> buttons)
{
    Queue<(bool[] indicators, int cost)> open = [];
    HashSet<int> closed = [];

    open.Enqueue((targetIndicators.Select(x => false).ToArray(), 0));

    while (open.Count > 0)
    {
        (bool[] indicators, int cost) = open.Dequeue();

        if (!closed.Add(indicators.Select((x, i) => x ? (1 << i) : 0).Sum()))
        {
            // We already found identical indicators in less button presses.
            continue;
        }

        if (indicators.SequenceEqual(targetIndicators))
        {
            return cost;
        }

        foreach (int[] buttonValues in buttons)
        {
            open.Enqueue((indicators.Select((x, i) => buttonValues.Contains(i) ? !x : x).ToArray(), cost + 1));
        }
    }

    return -1;
}

long SolveJoltage(List<int[]> buttons, int[] joltages)
{
    var context = new Microsoft.Z3.Context();
    var solver = context.MkOptimize();

    var buttonsPresses = buttons.Select((_, i) => context.MkIntConst("b" + i)).ToArray();
    foreach (var buttonPress in buttonsPresses)
    {
        solver.Add(buttonPress >= 0);
    }

    var totalButtons = context.MkAdd(buttonsPresses);
    solver.MkMinimize(totalButtons);

    var expectedValues = joltages.Select(x => context.MkInt(x)).ToArray();
    for (int i = 0; i < expectedValues.Length; i++)
    {
        var buttonsValues = buttonsPresses.Select((x, j) => context.MkMul(x, context.MkInt(buttons[j].Contains(i) ? 1 : 0)));
        solver.Add(context.MkEq(expectedValues[i], context.MkAdd(buttonsValues)));
    }

    if (solver.Check() != Microsoft.Z3.Status.SATISFIABLE)
    {
        throw new Exception($"Cannot solve joltages: {string.Join(",", joltages)}");
    }

    var model = solver.Model;
    return int.Parse(model.Eval(totalButtons).ToString());
}

var machines = File.ReadAllLines("input").Select(ParseMachine).ToList();

Console.WriteLine(machines.Select(x => SolveIndicators(x.indicators, x.buttons)).Sum());
Console.WriteLine(machines.Select(x => SolveJoltage(x.buttons, x.joltages)).Sum());
