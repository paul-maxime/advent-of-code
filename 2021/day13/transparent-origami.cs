string[] input = File.ReadAllText("input").Split("\n\n");

HashSet<(int x, int y)> paper = input[0].Split("\n")
    .Select(line => line.Split(",")
    .Select(int.Parse).ToArray())
    .Select(p => (p[0], p[1]))
    .ToHashSet();

List<(char direction, int at)> folds = input[1].Split("\n")
    .Where(x => x.Length > 0)
    .Select(line => (line[11], int.Parse(line.Split("=")[1])))
    .ToList();

foreach (var fold in folds)
{
    foreach (var point in paper.Where(p => (fold.direction == 'x' ? p.x : p.y) > fold.at).ToList())
    {
        paper.Add((
            fold.direction == 'x' ? fold.at * 2 - point.x : point.x,
            fold.direction == 'y' ? fold.at * 2 - point.y : point.y
        ));
        paper.Remove(point);
    }
    if (fold == folds[0])
    {
        Console.WriteLine($"After first fold: {paper.Count}");
    }
}

for (int y = 0; y <= paper.Select(p => p.y).Max(); y++)
{
    for (int x = 0; x <= paper.Select(p => p.x).Max(); x++)
    {
        Console.Write(paper.Contains((x, y)) ? "##" : "  ");
    }
    Console.WriteLine();
}
