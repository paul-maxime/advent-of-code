var components = File.ReadAllLines("input")
    .Select(line => line.Split(": "))
    .Select(line => (line[0], line[1].Split(" ").ToList()))
    .ToDictionary();

void BuildGraphFile()
{
    Console.WriteLine("graph {");
    foreach (var item in components)
    {
        foreach (var dest in item.Value)
        {
            Console.WriteLine("  " + item.Key + " -> " + dest);
        }
    }
    Console.WriteLine("}");
}

int CountComponents(string from, HashSet<string> visited)
{
    visited.Add(from);
    foreach (var item in components)
    {
        foreach (var dest in item.Value)
        {
            if (item.Key == from && !visited.Contains(dest))
            {
                CountComponents(dest, visited);
            }
            if (dest == from && !visited.Contains(item.Key))
            {
                CountComponents(item.Key, visited);
            }
        }
    }
    return visited.Count;
}

void DisconnectComponents(string itemA, string itemB)
{
    if (components.ContainsKey(itemA)) components[itemA].Remove(itemB);
    if (components.ContainsKey(itemB)) components[itemB].Remove(itemA);
}

BuildGraphFile();

// Use the graph file to easily see which components to remove.

DisconnectComponents("xkz", "mvv");
DisconnectComponents("tmt", "pnz");
DisconnectComponents("hxr", "gbc");

Console.WriteLine(CountComponents("hxr", []) * CountComponents("gbc", []));
