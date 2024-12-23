List<(string c1, string c2)> input = File.ReadAllLines("input")
    .Select(x => x.Split("-"))
    .Select(data => (data[0], data[1]))
    .ToList();

Dictionary<string, List<string>> computers = [];

foreach ((string c1, string c2) in input)
{
    if (!computers.ContainsKey(c1)) computers[c1] = [];
    computers[c1].Add(c2);
    if (!computers.ContainsKey(c2)) computers[c2] = [];
    computers[c2].Add(c1);
}

bool AreLinked(string c1, string c2)
{
    return computers[c1].Contains(c2);
}

int CountSetsOf3()
{
    HashSet<string> sets = [];
    foreach (string c in computers.Keys.Where(x => x.StartsWith('t')))
    {
        List<string> links = computers[c];
        for (int i = 0; i < links.Count; i++)
        {
            for (int j = 0; j < links.Count; j++)
            {
                if (i != j && AreLinked(links[i], links[j]))
                {
                    List<string> set = [c, links[i], links[j]];
                    sets.Add(string.Join(",", set.Order()));
                }
            }
        }
    }
    return sets.Count;
}

string ComputeBiggestGroup()
{
    List<List<string>> groups = [];
    foreach (string c1 in computers.Keys)
    {
        List<string> group = [c1];
        foreach (string c2 in computers[c1])
        {
            if (group.All(x => AreLinked(c2, x)))
            {
                group.Add(c2);
            }
        }
        groups.Add(group);
    }
    List<string> biggest = groups.OrderByDescending(x => x.Count).First();
    return string.Join(",", biggest.Order());
}


Console.WriteLine(CountSetsOf3());
Console.WriteLine(ComputeBiggestGroup());
