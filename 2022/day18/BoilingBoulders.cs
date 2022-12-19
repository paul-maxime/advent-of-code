const int MIN = 0;
const int MAX = 20;

var SIDES = new List<(int x, int y, int z)> {
    (-1, 0, 0), (1, 0, 0),
    (0, -1, 0), (0, 1, 0),
    (0, 0, -1), (0, 0, 1),
}.AsReadOnly();

HashSet<(int x, int y, int z)> cubes = File.ReadAllLines("input")
    .Select(line => line.Split(","))
    .Select(nums => (
        x: int.Parse(nums[0]),
        y: int.Parse(nums[1]),
        z: int.Parse(nums[2])
    )).ToHashSet();

bool IsTrapped((int x, int y, int z) air)
{
    var queue = new Queue<(int x, int y, int z)>();
    var visited = new HashSet<(int x, int y, int z)> { air };
    queue.Enqueue(air);

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        if (current.x < MIN || current.x > MAX || current.y < MIN || current.y > MAX || current.z < MIN || current.z > MAX) return false;

        foreach (var side in SIDES)
        {
            var maybe = (current.x + side.x, current.y + side.y, current.z + side.z);
            if (cubes.Contains(maybe)) continue;
            if (visited.Contains(maybe)) continue;
            visited.Add(maybe);
            queue.Enqueue(maybe);
        }
    }

    return true;
}

int AffectedSides((int x, int y, int z) air)
{
    return SIDES.Count(side => cubes.Contains((air.x + side.x, air.y + side.y, air.z + side.z)));
}

int allSidesCount = cubes
    .Select(cube => SIDES
        .Where(side => !cubes.Contains((cube.x + side.x, cube.y + side.y, cube.z + side.z)))
        .Count()
    ).Sum();

HashSet<(int x, int y, int z)> neighbors = cubes
    .SelectMany(cube => SIDES.Select(side => (cube.x + side.x, cube.y + side.y, cube.z + side.z)))
    .Where(cube => !cubes.Contains(cube))
    .ToHashSet();

int ignoredSides = neighbors.Where(air => IsTrapped(air)).Select(air => AffectedSides(air)).Sum();

foreach (var neighbor in neighbors)
{
    Console.WriteLine(neighbor + " " + IsTrapped(neighbor));
}

Console.WriteLine(allSidesCount);
Console.WriteLine(allSidesCount - ignoredSides);
