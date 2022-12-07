string[] input = File.ReadAllLines("input");

Stack<string> folders = new Stack<string>();
Dictionary<string, long> sizes = new Dictionary<string, long>();

foreach (string line in input)
{
    if (line.StartsWith("$ cd"))
    {
        string folder = line.Substring("$ cd ".Length);
        if (folder == "/")
        {
            folders.Clear();
            folders.Push("/");
        }
        else if (folder == "..")
        {
            folders.Pop();
        }
        else
        {
            folders.Push(string.Join("/", folders.Reverse().Append(folder)));
        }
    }
    else if (!line.StartsWith("$") && !line.StartsWith("dir"))
    {
        long size = long.Parse(line.Split(" ")[0]);
        foreach (string folder in folders)
        {
            sizes[folder] = sizes.GetValueOrDefault(folder) + size;
        }
    }
}

Console.WriteLine(sizes.Values.Where(x => x <= 100000).Sum());

long expected = 30000000 - (70000000 - sizes["/"]);
Console.WriteLine(sizes.Values.Where(x => x >= expected).Order().First());
