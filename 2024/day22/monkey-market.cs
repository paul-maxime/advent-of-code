List<long> input = File.ReadAllLines("input").Select(long.Parse).ToList();

long Next(long number)
{
    number ^= number * 64;
    number %= 16777216;
    number ^= number / 32;
    number %= 16777216;
    number ^= number * 2048;
    number %= 16777216;
    return number;
}

Dictionary<long, List<long>> prices = [];
Dictionary<long, List<long>> changes = [];

foreach (long number in input)
{
    prices[number] = [];
    changes[number] = [];

    long current = number;
    long previousPrice = number % 10;
    for (int i = 0; i < 2000; i++)
    {
        current = Next(current);
        long price = current % 10;

        prices[number].Add(price);
        changes[number].Add(price - previousPrice);

        previousPrice = price;
    }
}

long FindPrice(long number, long[] sequence)
{
    for (int i = 0; i < changes[number].Count - sequence.Length; i++)
    {
        int j;
        for (j = 0; j < sequence.Length; j++)
        {
            if (changes[number][i + j] != sequence[j]) break;
        }

        if (j == sequence.Length)
        {
            return prices[number][i + sequence.Length - 1];
        }
    }

    return 0;
}

long Generate(long number, int iterations)
{
    for (int i = 0; i < iterations; i++)
    {
        number = Next(number);
    }
    return number;
}

long ComputeMax(long i1)
{
    long max = 0;
    for (long i2 = -9; i2 <= 9; i2++)
    {
        Console.WriteLine($"Thread {i1} at {i2}");
        for (long i3 = -9; i3 <= 9; i3++)
        {
            for (long i4 = -9; i4 <= 9; i4++)
            {
                max = Math.Max(input.Select(n => FindPrice(n, [i1, i2, i3, i4])).Sum(), max);
            }
        }
    }
    return max;
}

Console.WriteLine(input.Select(n => Generate(n, 2000)).Sum());
Console.WriteLine(Enumerable.Range(-9, 19).AsParallel().Select(x => ComputeMax(x)).Max());

