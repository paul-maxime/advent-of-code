string[] input = File.ReadAllLines("input");

List<List<long>> numbers = input
    .SkipLast(1)
    .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(long.Parse).ToList())
    .ToList();

List<char> operations = input[^1]
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x[0])
    .ToList();

long ComputeUsingWrongNumbers()
{
    long total = 0;
    for (int i = 0; i < operations.Count; i++)
    {
        if (operations[i] == '+')
        {
            total += numbers.Select(x => x[i]).Sum();
        }
        else if (operations[i] == '*')
        {
            total += numbers.Select(x => x[i]).Aggregate((a, b) => a * b);
        }
    }
    return total;
}

long ComputeUsingRightToLeftNumbers()
{
    int currentPosition = 0;
    long total = 0;
    for (int i = 0; i < operations.Count; i++)
    {
        int longestDigit = numbers
            .Select(x => x[i].ToString().Length)
            .Max();

        IEnumerable<long> currentNumbers = Enumerable.Range(0, longestDigit)
            .Select(digit => long.Parse(
                string.Join("", input
                    .SkipLast(1)
                    .Select(line => line[currentPosition + digit]))
                    .Trim()
                )
            );

        if (operations[i] == '+')
        {
            total += currentNumbers.Sum();
        }
        else if (operations[i] == '*')
        {
            total += currentNumbers.Aggregate((a, b) => a * b);
        }

        currentPosition += longestDigit + 1;
    }
    return total;
}

Console.WriteLine(ComputeUsingWrongNumbers());
Console.WriteLine(ComputeUsingRightToLeftNumbers());
