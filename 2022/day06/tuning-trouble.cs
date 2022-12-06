string input = File.ReadAllText("input").Trim();

int GetMarker(int size) => Enumerable.Range(size - 1, input.Length)
    .Where(i => (new HashSet<char>(input.Skip(i - size + 1).Take(size))).Count == size)
    .First() + 1;

Console.WriteLine(GetMarker(4));
Console.WriteLine(GetMarker(14));
