List<(long result, List<long> operands)> equations = File.ReadAllLines("input")
    .Select(line => line.Split(": "))
    .Select(line => (
        result: long.Parse(line[0]),
        operands: line[1].Split(" ").Select(long.Parse).ToList()
    ))
    .ToList();

bool IsResultPossible(long result, List<long> operands, bool canConcat, int index = 0, long current = 0)
{
    if (index == 0)
    {
        return IsResultPossible(result, operands, canConcat, 1, operands[0]);
    }

    if (index >= operands.Count)
    {
        return result == current;
    }

    long sumResult = current + operands[index];
    long productResult = current * operands[index];
    long concatResult = long.Parse(current + "" + operands[index]);

    return (
        IsResultPossible(result, operands, canConcat, index + 1, sumResult) ||
        IsResultPossible(result, operands, canConcat, index + 1, productResult) ||
        canConcat && IsResultPossible(result, operands, canConcat, index + 1, concatResult)
    );
}

long ComputeEquations(bool canConcat)
{
    return equations
        .Where(eq => IsResultPossible(eq.result, eq.operands, canConcat))
        .Select(eq => eq.result)
        .Sum();
}

Console.WriteLine(ComputeEquations(false));
Console.WriteLine(ComputeEquations(true));
