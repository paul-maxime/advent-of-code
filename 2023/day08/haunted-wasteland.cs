string[] input = File.ReadAllLines("input");

string instructions = input[0];

Dictionary<string, (string left, string right)> nodes = input.Skip(2)
    .Select(line => line.Replace(" = (", ",").Replace(", ", ",").Replace(")", "").Split(","))
    .ToDictionary(x => x[0], x => (left: x[1], right: x[2]));

long FollowPath(string from, bool isGhost)
{
    string current = from;
    long step = 0;
    while (isGhost ? !current.EndsWith('Z') : current != "ZZZ")
    {
        char direction = instructions[(int)(step % instructions.Length)];
        current = direction == 'L' ? nodes[current].left : nodes[current].right;
        step += 1;
    }
    return step;
}

// https://stackoverflow.com/a/18541902
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

long FollowGhostPath()
{
    string[] current = nodes.Keys.Where(x => x.EndsWith('A')).ToArray();
    long[] distances = current.Select(x => FollowPath(x, true)).ToArray();
    long gcd = GCD(distances[0], distances[1]);

    return distances.Select(x => x / gcd).Aggregate((a, b) => a * b) * gcd;
}

Console.WriteLine(FollowPath("AAA", false));
Console.WriteLine(FollowGhostPath());
