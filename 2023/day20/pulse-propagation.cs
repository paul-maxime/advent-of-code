var modules = File.ReadAllLines("input")
    .Select(line => line.Split(" -> "))
    .Select(line => (
        type: line[0][0] == '%' ? "%" : line[0][0] == '&' ? "&" : "",
        name: line[0].Replace("%", "").Replace("&", ""),
        destinations: line[1].Split(", ")
    )).ToDictionary(x => x.name);

Dictionary<string, bool> flipflopMemory = modules.Values
    .Where(x => x.type == "%")
    .Select(x => (x.name, false))
    .ToDictionary();

Dictionary<string, Dictionary<string, bool>> conjunctionMemory = modules.Values
    .Where(x => x.type == "&")
    .Select(x => (
        x.name,
        modules.Values
            .Where(y => y.destinations.Contains(x.name))
            .Select(y => (y.name, false))
            .ToDictionary()
    ))
    .ToDictionary();

string prerequisiteModule = modules.Values.Where(x => x.destinations[0] == "rx").First().name;
Dictionary<string, long> prerequisiteCycles = [];

long low = 0;
long high = 0;

for (long i = 0; i < 10000; i++)
{
    Queue<(string from, string module, bool pulse)> actions = [];
    actions.Enqueue(("button", "broadcaster", false));

    while (actions.Count > 0)
    {
        var action = actions.Dequeue();

        if (action.module == prerequisiteModule && action.pulse && !prerequisiteCycles.ContainsKey(action.from))
        {
            prerequisiteCycles.Add(action.from, i + 1);
        }

        if (i < 1000)
        {
            if (action.pulse) high += 1;
            else low += 1;
        }

        if (!modules.ContainsKey(action.module)) continue;

        var module = modules[action.module];
        bool pulse = action.pulse;

        if (module.type == "%")
        {
            if (!action.pulse)
            {
                flipflopMemory[module.name] = !flipflopMemory[module.name];
                pulse = flipflopMemory[module.name];
            }
            else
            {
                continue;
            }
        }
        else if (module.type == "&")
        {
            conjunctionMemory[module.name][action.from] = action.pulse;
            pulse = !conjunctionMemory[module.name].Values.All(x => x);
        }

        foreach (var next in module.destinations)
        {
            actions.Enqueue((module.name, next, pulse));
        }
    }
}

Console.WriteLine(low * high);
Console.WriteLine(prerequisiteCycles.Values.Aggregate((a, b) => a * b));
