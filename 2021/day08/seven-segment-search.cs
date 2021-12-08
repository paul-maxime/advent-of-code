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

int CountEasyDigits(string[][] input) => input
    .SelectMany(x => x.TakeLast(4))
    .Where(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7)
    .Count();

static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length) =>
    length == 1 ?
        list.Select(t => new T[] { t }) :
        GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));

string PermutateString(string input, string to) =>
    string.Join("", input.Select(x => WIRES.IndexOf(x)).Select(x => to[x]).OrderBy(x => x));

bool IsInputValid(string[] input, string permutation) => input
    .Select(x => PermutateString(x, permutation))
    .All(x => DIGITS.ContainsKey(x));

int SumAllOutputs(string[][] input) => GetPermutations(WIRES, WIRES.Length)
    .Select(x => string.Join("", x))
    .SelectMany(permutation => input.Where(x => IsInputValid(x, permutation)).Select(line => (permutation, line)))
    .Select(result => int.Parse(string.Join("",
        result.line.TakeLast(4).Select(x => PermutateString(x, result.permutation)).Select(x => DIGITS[x].ToString())
    ))).Sum();

Console.WriteLine(CountEasyDigits(input));
Console.WriteLine(SumAllOutputs(input));
