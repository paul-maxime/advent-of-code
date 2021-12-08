string[][] input = File.ReadAllLines("input")
    .Select(line => line
        .Replace(" | ", " ")
        .Split(" ")
        .Select(word => new string(word.ToCharArray().OrderBy(x => x).ToArray()))
        .ToArray()
    ).ToArray();

const string WIRES = "abcdefg";

Dictionary<string, int> DIGITS = new Dictionary<string, int> {
    { "abcdeg", 0 },
    { "ab", 1 },
    { "acdfg", 2 },
    { "abcdf", 3 },
    { "abef", 4 },
    { "bcdef", 5 },
    { "bcdefg", 6 },
    { "abd", 7 },
    { "abcdefg", 8 },
    { "abcdef", 9 },
};

int CountEasyDigits(string[][] input)
{
    return input
        .SelectMany(x => x.TakeLast(4))
        .Where(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7)
        .Count();
}

static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
{
    if (length == 1) return list.Select(t => new T[] { t });

    return GetPermutations(list, length - 1)
        .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
}

string PermutateString(string input, string to)
{
    return string.Join("", input.Select(x => WIRES.IndexOf(x)).Select(x => to[x]).OrderBy(x => x));
}

bool IsInputValid(string[] input, string permutation)
{
  return input.Select(x => PermutateString(x, permutation)).All(x => DIGITS.ContainsKey(x));
}

int SumAllOutputs(string[][] input)
{
    int sum = 0;
    foreach (string permutation in GetPermutations(WIRES, WIRES.Length).Select(x => string.Join("", x)))
    {
        foreach (var line in input.Where(x => IsInputValid(x, permutation)))
        {
            sum += int.Parse(string.Join("",
                line.TakeLast(4).Select(x => PermutateString(x, permutation)).Select(x => DIGITS[x].ToString())
            ));
        }
    }
    return sum;
}

Console.WriteLine(CountEasyDigits(input));
Console.WriteLine(SumAllOutputs(input));
