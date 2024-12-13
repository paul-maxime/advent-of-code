string[] map = File.ReadAllLines("input");
int mapHeight = map.Length;
int mapWidth = map[0].Length;

char CellAt(int x, int y)
{
    if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return '.';
    return map[y][x];
}

void DetectRegion(int x, int y, HashSet<(int x, int y)> visited, HashSet<(int x, int y)> region)
{
    visited.Add((x, y));
    region.Add((x, y));

    if (!visited.Contains((x + 1, y)) && CellAt(x + 1, y) == CellAt(x, y)) DetectRegion(x + 1, y, visited, region);
    if (!visited.Contains((x - 1, y)) && CellAt(x - 1, y) == CellAt(x, y)) DetectRegion(x - 1, y, visited, region);
    if (!visited.Contains((x, y + 1)) && CellAt(x, y + 1) == CellAt(x, y)) DetectRegion(x, y + 1, visited, region);
    if (!visited.Contains((x, y - 1)) && CellAt(x, y - 1) == CellAt(x, y)) DetectRegion(x, y - 1, visited, region);
}

void VisitBorders(int x, int y, int direction, HashSet<(int x, int y, int direction)> borders, HashSet<(int x, int y, int direction)> visited)
{
    visited.Add((x, y, direction));

    if (!visited.Contains((x + 1, y, direction)) && borders.Contains((x + 1, y, direction))) VisitBorders(x + 1, y, direction, borders, visited);
    if (!visited.Contains((x - 1, y, direction)) && borders.Contains((x - 1, y, direction))) VisitBorders(x - 1, y, direction, borders, visited);
    if (!visited.Contains((x, y + 1, direction)) && borders.Contains((x, y + 1, direction))) VisitBorders(x, y + 1, direction, borders, visited);
    if (!visited.Contains((x, y - 1, direction)) && borders.Contains((x, y - 1, direction))) VisitBorders(x, y - 1, direction, borders, visited);
}

long CountUniqueBorders(HashSet<(int x, int y, int direction)> borders)
{
    HashSet<(int x, int y, int direction)> visited = [];
    long count = 0;
    foreach (var (x, y, direction) in borders)
    {
        if (!visited.Contains((x, y, direction)))
        {
            VisitBorders(x, y, direction, borders, visited);
            count += 1;
        }
    }
    return count;
}

long RegionCost(HashSet<(int x, int y)> region, bool hasDiscount)
{
    HashSet<(int x, int y, int direction)> borders = [];

    foreach ((int x, int y) in region)
    {
        if (!region.Contains((x + 1, y))) borders.Add((x + 1, y, 1));
        if (!region.Contains((x - 1, y))) borders.Add((x - 1, y, 2));
        if (!region.Contains((x, y + 1))) borders.Add((x, y + 1, 3));
        if (!region.Contains((x, y - 1))) borders.Add((x, y - 1, 4));
    }

    if (hasDiscount)
    {
        return CountUniqueBorders(borders) * region.Count;
    }
    else
    {
        return borders.Count * region.Count;
    }
}

long ComputeTotalCost(bool hasDiscount)
{
    HashSet<(int x, int y)> visited = [];
    long total = 0;
    for (int x = 0; x < mapWidth; x++)
    {
        for (int y = 0; y < mapHeight; y++)
        {
            if (!visited.Contains((x, y)))
            {
                HashSet<(int x, int y)> region = [];
                DetectRegion(x, y, visited, region);
                long cost = RegionCost(region, hasDiscount);
                total += cost;
            }
        }
    }
    return total;
}

Console.WriteLine(ComputeTotalCost(false));
Console.WriteLine(ComputeTotalCost(true));
