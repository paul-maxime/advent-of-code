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
    return SIDES.All(side => IsTrappedFromSide(air, side));
}

bool IsTrappedFromSide((int x, int y, int z) air, (int x, int y, int z) delta)
{
    if (air.x < MIN || air.x > MAX || air.y < MIN || air.y > MAX || air.z < MIN || air.z > MAX) return false;
    if (cubes.Contains(air)) return true;
    return IsTrappedFromSide((air.x + delta.x, air.y + delta.y, air.z + delta.z), delta);
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

Console.WriteLine(allSidesCount);
Console.WriteLine(allSidesCount - ignoredSides);
