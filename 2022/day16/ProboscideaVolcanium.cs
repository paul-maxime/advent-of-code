var regex = new System.Text.RegularExpressions.Regex(
    @"^Valve (..) has flow rate=(\d+); tunnels? leads? to valves? ([A-Z, ]+)$"
);

var valves = File.ReadAllLines("input")
    .Select(line => regex.Match(line))
    .Select(matches => (
        id: matches.Groups[1].Value,
        flow: int.Parse(matches.Groups[2].Value),
        tunnels: matches.Groups[3].Value.Split(", ")
    ))
    .ToDictionary(x => x.id);

var usefulValves = valves.Values.Where(x => x.flow > 0).ToList();

var paths = new Dictionary<(string from, string to), string>();

void PrecomputePaths()
{
    foreach (var valveA in valves.Values)
    {
        foreach (var valveB in valves.Values)
        {
            if (valveA.id == valveB.id) continue;
            paths.Add((valveA.id, valveB.id), Pathfind(valveA.id, valveB.id));
        }
    }
}

string Pathfind(string from, string to)
{
    var queue = new Queue<(string previous, string current)>();
    var explored = new HashSet<string>() { to };
    queue.Enqueue(("", to));
    while (queue.Count > 0)
    {
        (string previous, string current) = queue.Dequeue();
        if (current == from)
        {
            return previous;
        }
        foreach (string next in valves[current].tunnels)
        {
            if (explored.Contains(next)) continue;
            explored.Add(next);
            queue.Enqueue((current, next));
        }
    }
    throw new Exception("no path");
}

PrecomputePaths();

Console.WriteLine("paths precomputed");

var cache = new Dictionary<(string current, string open, int pressure, int minutes), long>();

long RecursiveValveProcessingV1(string current, HashSet<string> open, List<string> remaining, int pressure, int minutes)
{
    foreach (string id in open) pressure += valves[id].flow;
    minutes += 1;

    long cachedValue;
    if (cache.TryGetValue((current, string.Join(",", open.Order()), pressure, minutes), out cachedValue))
    {
        return cachedValue;
    }

    long maxPressure = 0;

    if (minutes == 30 || open.Count == usefulValves.Count)
    {
        maxPressure = pressure;
        foreach (string id in open) maxPressure += valves[id].flow * (30 - minutes);
    }
    else
    {
        var nextTunnels = remaining.Where(id => id != current).Select(id => paths[(current, id)]).Distinct();
        foreach (var next in nextTunnels)
        {
            maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV1(next, open, remaining, pressure, minutes));
        }
        if (valves[current].flow > 0 && !open.Contains(current))
        {
            HashSet<string> newOpen = new HashSet<string>(open);
            newOpen.Add(current);
            List<string> newRemaining = remaining.Where(x => x != current).ToList();
            maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV1(current, newOpen, newRemaining, pressure, minutes));
        }
    }

    cache.Add((current, string.Join(",", open.Order()), pressure, minutes), maxPressure);
    return maxPressure;
}

var cache2 = new Dictionary<(string current1, string current2, string open, int pressure, int minutes), long>();

long RecursiveValveProcessingV2(string current1, string current2, HashSet<string> open, List<string> remaining, int pressure, int minutes)
{
    foreach (string id in open) pressure += valves[id].flow;
    minutes += 1;

    long cachedValue;
    if (cache2.TryGetValue((current1, current2, string.Join(",", open.Order()), pressure, minutes), out cachedValue))
    {
        return cachedValue;
    }

    long maxPressure = 0;

    if (minutes == 13 || open.Count == usefulValves.Count)
    {
        maxPressure = pressure;
        foreach (string id in open) maxPressure += valves[id].flow * (13 - minutes);
    }
    else
    {
        var nextTunnels1 = remaining.Where(id => id != current1).Select(id => paths[(current1, id)]).Distinct();
        var nextTunnels2 = remaining.Where(id => id != current2).Select(id => paths[(current2, id)]).Distinct();
        foreach (var next1 in nextTunnels1)
        {
            foreach (var next2 in nextTunnels2)
            {
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV2(next1, next2, open, remaining, pressure, minutes));
            }
        }
        if (valves[current1].flow > 0 && !open.Contains(current1))
        {
            HashSet<string> newOpen = new HashSet<string>(open);
            newOpen.Add(current1);
            List<string> newRemaining = remaining.Where(x => x != current1).ToList();
            foreach (var next2 in nextTunnels2)
            {
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV2(current1, next2, newOpen, newRemaining, pressure, minutes));
            }
        }
        if (valves[current2].flow > 0 && !open.Contains(current2))
        {
            HashSet<string> newOpen = new HashSet<string>(open);
            newOpen.Add(current2);
            List<string> newRemaining = remaining.Where(x => x != current2).ToList();
            foreach (var next1 in nextTunnels1)
            {
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV2(next1, current2, newOpen, newRemaining, pressure, minutes));
            }
        }
        if (valves[current1].flow > 0 && !open.Contains(current1) && valves[current2].flow > 0 && !open.Contains(current2))
        {
            HashSet<string> newOpen = new HashSet<string>(open);
            newOpen.Add(current1);
            newOpen.Add(current2);
            List<string> newRemaining = remaining.Where(x => x != current1 && x != current2).ToList();
            maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV2(current1, current2, newOpen, newRemaining, pressure, minutes));
        }
    }

    cache2.Add((current1, current2, string.Join(",", open.Order()), pressure, minutes), maxPressure);
    return maxPressure;
}

long RecursiveValveProcessingV3(string current, HashSet<string> open, List<string> remaining, int pressure, int minutes, string target = "")
{
    foreach (string id in open) pressure += valves[id].flow;
    minutes += 1;

    long maxPressure = 0;

    if (minutes == 30 || open.Count == usefulValves.Count)
    {
        maxPressure = pressure;
        foreach (string id in open) maxPressure += valves[id].flow * (30 - minutes);
    }
    else
    {
        if (target == "")
        {
            foreach (string newTarget in remaining)
            {
                if (newTarget == current) continue;
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV3(paths[(current, newTarget)], open, remaining, pressure, minutes, newTarget));
            }
        }
        else if (target == current)
        {
            if (!open.Contains(current))
            {
                HashSet<string> newOpen = new HashSet<string>(open);
                newOpen.Add(current);
                List<string> newRemaining = remaining.Where(x => x != current).ToList();
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV3(current, newOpen, newRemaining, pressure, minutes));
            }
        }
        else
        {
            maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV3(paths[(current, target)], open, remaining, pressure, minutes, target));
        }
    }

    return maxPressure;
}

long globalMaxPressure = 0;

long RecursiveValveProcessingV4(List<(string current, string target)> players, int currentPlayer, HashSet<string> open, List<string> remaining, int pressure, int minutes)
{
    if (currentPlayer == 0)
    {
        foreach (string id in open) pressure += valves[id].flow;
        minutes += 1;
    }

    string current = players[currentPlayer].current;
    string target = players[currentPlayer].target;

    long maxPressure = 0;

    if (minutes == 26 || open.Count == usefulValves.Count)
    {
        maxPressure = pressure;
        foreach (string id in open) maxPressure += valves[id].flow * (26 - minutes);
    }
    else
    {
        if (target == "")
        {
            foreach (string newTarget in remaining)
            {
                if (newTarget == current || newTarget == players[1 - currentPlayer].target) continue;
                var newPlayers = players.Select((p, i) => i == currentPlayer ? (paths[(current, newTarget)], newTarget) : p).ToList();
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV4(newPlayers, (currentPlayer + 1) % players.Count, open, remaining, pressure, minutes));
            }
        }
        else if (target == current)
        {
            if (!open.Contains(current))
            {
                HashSet<string> newOpen = new HashSet<string>(open);
                newOpen.Add(current);
                List<string> newRemaining = remaining.Where(x => x != current).ToList();

                var newPlayers = players.Select((p, i) => i == currentPlayer ? (current, "") : p).ToList();
                maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV4(newPlayers, (currentPlayer + 1) % players.Count, newOpen, newRemaining, pressure, minutes));
            }
        }
        else
        {
            var newPlayers = players.Select((p, i) => i == currentPlayer ? (paths[(current, target)], target) : p).ToList();
            maxPressure = Math.Max(maxPressure, RecursiveValveProcessingV4(newPlayers, (currentPlayer + 1) % players.Count, open, remaining, pressure, minutes));
        }
    }

    if (maxPressure > globalMaxPressure)
    {
        globalMaxPressure = maxPressure;
        Console.WriteLine("max pressure is now " + globalMaxPressure);
    }
    return maxPressure;
}

Console.WriteLine(RecursiveValveProcessingV3("AA", new HashSet<string>(), usefulValves.Select(x => x.id).ToList(), 0, 0));

var sw = new System.Diagnostics.Stopwatch();
sw.Start();

long result = RecursiveValveProcessingV4(new List<(string current, string target)> { ("AA", ""), ("AA", "") }, 0, new HashSet<string>(), usefulValves.Select(x => x.id).Reverse().ToList(), 0, 0);

sw.Stop();

Console.WriteLine(result + " in " + sw.ElapsedMilliseconds + "ms");
