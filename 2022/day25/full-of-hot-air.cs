long SnafuToDecimal(string snafu)
{
    long result = 0;
    long multiplier = 1;
    for (int i = snafu.Length - 1; i >= 0; i--)
    {
        result += snafu[i] switch {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '-' => -1,
            '=' => -2,
            _ => throw new Exception("Invalid snafu character")
        } * multiplier;
        multiplier *= 5;
    }
    return result;
}

string DecimalToSnafu(long number)
{
    if (number == 0) return "0";
    string result = "";
    long remainer = 0;
    while (number > 0 || remainer != 0)
    {
        long current = number % 5 + remainer;
        remainer = 0;
        number /= 5;
        if (current > 2)
        {
            current -= 5;
            remainer = 1;
        }
        result = current switch {
            0 => '0',
            1 => '1',
            2 => '2',
            -1 => '-',
            -2 => '=',
            _ => throw new Exception("Unexpected snafu digit")
        } + result;
    }
    return result;
}

long sum = File.ReadAllLines("input")
    .Select(line => SnafuToDecimal(line))
    .Sum();

Console.WriteLine(sum);
Console.WriteLine(DecimalToSnafu(sum));
