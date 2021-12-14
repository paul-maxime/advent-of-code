string[] input = File.ReadAllLines("input");

string sequence = input[0];

Dictionary<(char, char), char> rules = input.Skip(2)
    .Select(x => (x[0], x[1], x[6]))
    .ToDictionary(x => (x.Item1, x.Item2), x => x.Item3);

Dictionary<(char, char), long> pairs = rules.Keys
    .ToDictionary(x => x, _ => 0L);

Dictionary<char, long> elements = pairs.Keys
    .SelectMany(x => new [] { x.Item1, x.Item2 })
    .Distinct()
    .ToDictionary(x => x, _ => 0L);

foreach (char c in sequence)
{
    elements[c] += 1;
}

for (int i = 0; i < sequence.Length - 1; i++)
{
    pairs[(sequence[i], sequence[i + 1])] += 1;
}

for (int step = 1; step <= 40; step++)
{
    Dictionary<(char, char), long> newPairs = rules.Keys.ToDictionary(x => x, _ => 0L);
    foreach (var pair in pairs)
    {
        char newElement = rules[(pair.Key.Item1, pair.Key.Item2)];
        elements[newElement] += pair.Value;
        newPairs[(pair.Key.Item1, newElement)] += pair.Value;
        newPairs[(newElement, pair.Key.Item2)] += pair.Value;
    }
    pairs = newPairs;

    if (step == 10 || step == 40)
    {
        Console.WriteLine(elements.MaxBy(x => x.Value).Value - elements.MinBy(x => x.Value).Value);
    }
}
