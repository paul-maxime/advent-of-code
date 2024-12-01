List<(long left, long right)> lines = File.ReadAllLines("input")
    .Select(line => line.Split("   "))
    .Select(data => (left: long.Parse(data[0]), right: long.Parse(data[1])))
    .ToList();

List<long> leftColumn = lines.Select(x => x.left).Order().ToList();
List<long> rightColumn = lines.Select(x => x.right).Order().ToList();

long distanceScore = Enumerable.Range(0, lines.Count)
    .Select(i => Math.Abs(leftColumn[i] - rightColumn[i]))
    .Sum();
Console.WriteLine(distanceScore);

long similarityScore = leftColumn
    .Select(left => left * rightColumn.Count(right => left == right))
    .Sum();
Console.WriteLine(similarityScore);
