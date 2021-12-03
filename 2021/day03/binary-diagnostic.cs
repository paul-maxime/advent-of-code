string[] input = File.ReadAllLines("input");

(int, int) GetGammaAndEpsilon(string[] input)
{
    string gamma = "";
    string epsilon = "";

    for (int i = 0; i < input[0].Length; i++)
    {
        int zeroes = input.Where(x => x[i] == '0').Count();
        int ones = input.Where(x => x[i] == '1').Count();

        gamma += ones > zeroes ? "1" : "0";
        epsilon += ones < zeroes ? "1" : "0";
    }

    return (Convert.ToInt32(gamma, 2), Convert.ToInt32(epsilon, 2));
}

int GetBinaryRating(string[] input, bool greatest)
{
    string[] remaining = input;

    for (int i = 0; i < input[0].Length; i++)
    {
        int zeroes = remaining.Where(x => x[i] == '0').Count();
        int ones = remaining.Where(x => x[i] == '1').Count();

        char expected = ones >= zeroes ? (greatest ? '1' : '0') : (greatest ? '0' : '1');
        remaining = remaining.Where(x => x[i] == expected).ToArray();

        if (remaining.Length == 1) break;
    }

    return Convert.ToInt32(remaining[0], 2);
}

(int gamma, int epsilon) = GetGammaAndEpsilon(input);

Console.WriteLine($"gamma {gamma} x epsilon {epsilon} = {gamma * epsilon} power consumption");

int oxygen = GetBinaryRating(input, true);
int carbon = GetBinaryRating(input, false);

Console.WriteLine($"oxygen {oxygen} x carbon {carbon} = {oxygen * carbon} life support");
