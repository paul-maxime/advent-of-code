const int MAX_MINUTES = 32;
const int MAX_BLUEPRINTS = 3;

var blueprints = File.ReadAllLines("input")
    .Select(line => new System.Text.RegularExpressions.Regex(@"(\d+)").Matches(line))
    .Select(matches => matches.Select(match => int.Parse(match.Value)).ToArray())
    .Select(matches => new Blueprint {
        id = matches[0],
        ore = matches[1],
        clay = matches[2],
        obsidian = (ore: matches[3], clay: matches[4]),
        geode = (ore: matches[5], obsidian: matches[6])
    }).ToList();

Console.WriteLine("Starting!");

List<Task> tasks = new List<Task>();
foreach (var blueprint in blueprints.Take(MAX_BLUEPRINTS))
{
    tasks.Add(Task.Run(() => {
        new Solver().Solve(blueprint, new Resources() { ore = 1 }, new Resources(), MAX_MINUTES);
    }));
}

Task.WaitAll(tasks.ToArray());

class Blueprint
{
    public int id;
    public int ore;
    public int clay;
    public (int ore, int clay) obsidian;
    public (int ore, int obsidian) geode;

    public int MaxRequiredOre() => Math.Max(ore, Math.Max(clay, Math.Max(obsidian.ore, geode.ore)));
}

struct Resources
{
    public int ore;
    public int clay;
    public int obsidian;
    public int geode;

    public override string ToString() => $"(ore: {ore}, clay: {clay}, obsidian: {obsidian}, geode: {geode})";
}

class Solver
{
    private long checks = 0;
    private long best = 0;

    public int Solve(Blueprint blueprint, Resources robots, Resources resources, int minutes)
    {
        if (minutes == 0)
        {
            return resources.geode;
        }

        int max = 0;

        int possibleActions = 0;

        for (int action = 0; action <= 4; action++)
        {
            Resources newRobots = robots;
            Resources newResources = resources;

            // spend resources for robots if possible
            if (action == 0)
            {
                newRobots.ore += 1;
                newResources.ore -= blueprint.ore;
                if (newRobots.ore > blueprint.MaxRequiredOre()) continue;
                if (newResources.ore < 0) continue;
            }
            if (action == 1)
            {
                newRobots.clay += 1;
                newResources.ore -= blueprint.clay;
                if (newRobots.clay > blueprint.obsidian.clay) continue;
                if (newResources.ore < 0) continue;
            }
            if (action == 2)
            {
                newRobots.obsidian += 1;
                newResources.ore -= blueprint.obsidian.ore;
                newResources.clay -= blueprint.obsidian.clay;
                if (newRobots.obsidian > blueprint.geode.obsidian) continue;
                if (newResources.ore < 0) continue;
                if (newResources.clay < 0) continue;
            }
            if (action == 3)
            {
                newRobots.geode += 1;
                newResources.ore -= blueprint.geode.ore;
                newResources.obsidian -= blueprint.geode.obsidian;
                if (newResources.ore < 0) continue;
                if (newResources.obsidian < 0) continue;
            }
            if (action == 4 && possibleActions == 4)
            {
                continue;
            }

            // add new resources
            newResources.ore += robots.ore;
            newResources.clay += robots.clay;
            newResources.obsidian += robots.obsidian;
            newResources.geode += robots.geode;

            // add new robots
            max = Math.Max(max, Solve(blueprint, newRobots, newResources, minutes - 1));
            possibleActions++;
        }

        this.checks++;
        if (max > this.best || (this.checks % 100000000) == 0)
        {
            this.best = Math.Max(max, this.best);
            Console.WriteLine($"{blueprint.id} -> {this.best} ({this.checks} checks)");
        }

        return max;
    }
}