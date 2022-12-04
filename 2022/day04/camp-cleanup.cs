List<int[]> assignments = File.ReadAllLines("input")
    .Select(x => x
        .Replace("-", ",")
        .Split(",")
        .Select(x => int.Parse(x))
        .ToArray()
    ).ToList();

Console.WriteLine(assignments
    .Where(x => x[0] >= x[2] && x[1] <= x[3] || x[2] >= x[0] && x[3] <= x[1])
    .Count()
);

Console.WriteLine(assignments
    .Where(x => x[0] <= x[3] && x[2] <= x[1])
    .Count()
);
