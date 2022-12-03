string[] rucksacks = File.ReadAllLines("input");

int ItemToPriority(char c) => char.IsUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1);

Console.WriteLine(rucksacks
    .Select(line => (
        line.Substring(0, line.Length / 2),
        line.Substring(line.Length / 2, line.Length / 2)
    ))
    .Select(bag => bag.Item1.Intersect(bag.Item2).First())
    .Select(c => ItemToPriority(c))
    .Sum());

Console.WriteLine(rucksacks
    .Chunk(3)
    .Select(group => group[0].Intersect(group[1]).Intersect(group[2]).First())
    .Select(c => ItemToPriority(c))
    .Sum());
