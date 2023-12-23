var slabs = File.ReadAllLines("input")
    .Select(line => line.Split("~").Select(coords => coords.Split(",").Select(int.Parse).ToArray()).ToArray())
    .Select(line => (
        from: (x: line[0][0], y: line[0][1], z: line[0][2]),
        to: (x: line[1][0], y: line[1][1], z: line[1][2])
    )).ToList();

HashSet<(int x, int y, int z)> map = [];

IEnumerable<(int x, int y, int z)> GetPointsBetween((int x, int y, int z) from, (int x, int y, int z) to)
{
    yield return (from.x, from.y, from.z);
    for (int x = from.x, y = from.y, z = from.z; x != to.x || y != to.y || z != to.z;)
    {
        x += Math.Sign(to.x - x);
        y += Math.Sign(to.y - y);
        z += Math.Sign(to.z - z);
        yield return (x, y, z);
    }
}

void AddToMap(HashSet<(int x, int y, int z)> map, (int x, int y, int z) from, (int x, int y, int z) to)
{
    foreach (var point in GetPointsBetween(from, to))
    {
        map.Add(point);
    }
}

void RemoveFromMap(HashSet<(int x, int y, int z)> map, (int x, int y, int z) from, (int x, int y, int z) to)
{
    foreach (var point in GetPointsBetween(from, to))
    {
        map.Remove(point);
    }
}

bool CanFitInMap(HashSet<(int x, int y, int z)> map, (int x, int y, int z) from, (int x, int y, int z) to)
{
    return GetPointsBetween(from, to).All(point => point.z >= 1 && !map.Contains(point));
}

bool CanAnySlabMove(HashSet<(int x, int y, int z)> map)
{
    foreach (var slab in slabs)
    {
        var movedSlab = (to: (slab.from.x, slab.from.y, slab.from.z - 1), from: (slab.to.x, slab.to.y, slab.to.z - 1));
        RemoveFromMap(map, slab.from, slab.to);
        bool canFit = CanFitInMap(map, movedSlab.from, movedSlab.to);
        AddToMap(map, slab.from, slab.to);
        if (canFit) return true;
    }
    return false;
}

int ApplyGravity(HashSet<(int x, int y, int z)> map, ref List<((int x, int y, int z) from, (int x, int y, int z) to)> slabs)
{
    bool gravity = true;
    HashSet<int> movedIndexes = [];
    while (gravity)
    {
        gravity = false;
        List<((int x, int y, int z) from, (int x, int y, int z) to)> newList = [];

        for (int i = 0; i < slabs.Count; i++)
        {
            var slab = slabs[i];
            var movedSlab = (to: (slab.from.x, slab.from.y, slab.from.z - 1), from: (slab.to.x, slab.to.y, slab.to.z - 1));
            RemoveFromMap(map, slab.from, slab.to);
            if (CanFitInMap(map, movedSlab.from, movedSlab.to))
            {
                movedIndexes.Add(i);
                AddToMap(map, movedSlab.from, movedSlab.to);
                gravity = true;
                newList.Add(movedSlab);
            }
            else
            {
                AddToMap(map, slab.from, slab.to);
                newList.Add(slab);
            }
        }

        slabs = newList;
    }
    return movedIndexes.Count;
}

void InitMapAndGravity()
{
    foreach (var slab in slabs)
    {
        AddToMap(map, slab.from, slab.to);
    }
    ApplyGravity(map, ref slabs);
}

int CountStableSlabs()
{
    int stableCount = 0;
    foreach (var slab in slabs.ToArray())
    {
        slabs.Remove(slab);
        RemoveFromMap(map, slab.from, slab.to);

        if (!CanAnySlabMove(map)) stableCount += 1;

        AddToMap(map, slab.from, slab.to);
        slabs.Add(slab);
    }
    return stableCount;
}

int CountChainReactions()
{
    int totalDestruction = 0;

    foreach (var slab in slabs)
    {
        var clonedMap = map.ToHashSet();
        var clonedSlabs = slabs.ToList();

        clonedSlabs.Remove(slab);
        RemoveFromMap(clonedMap, slab.from, slab.to);

        totalDestruction += ApplyGravity(clonedMap, ref clonedSlabs);
    }

    return totalDestruction;
}

InitMapAndGravity();
Console.WriteLine(CountStableSlabs());
Console.WriteLine(CountChainReactions());
