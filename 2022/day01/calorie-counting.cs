int[][] calories = File.ReadAllText("input")
    .Split("\n\n")
    .Select(x => x.Split("\n").Where(x => x.Length > 0).Select(x => int.Parse(x)).ToArray())
    .ToArray();

Console.WriteLine(calories.Select(x => x.Sum()).Max());
Console.WriteLine(calories.Select(x => x.Sum()).OrderDescending().Take(3).Sum());
