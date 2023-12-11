string[] map = File.ReadAllLines("input");

string NORTH_PIPES = "S|LJ";
string SOUTH_PIPES = "S|7F";
string WEST_PIPES = "S-J7";
string EAST_PIPES = "S-LF";

int mapHeight = map.Length;
int mapWidth = map[0].Length;
int zoomedMapHeight = mapHeight * 2;
int zoomedMapWidth = mapWidth * 2;

(int x, int y) FindStart() => Enumerable.Range(0, mapWidth)
    .SelectMany(x => Enumerable.Range(0, mapHeight)
        .Select(y => (x, y, c: map[y][x]))
        .Where(cell => cell.c == 'S'))
    .Select(cell => (cell.x, cell.y))
    .First();

(int x, int y) FindNextCell((int x, int y)? previous, (int x, int y) current)
{
    char currentType = map[current.y][current.x];

    (int x, int y) north = (current.x, current.y - 1);
    (int x, int y) south = (current.x, current.y + 1);
    (int x, int y) west = (current.x - 1, current.y);
    (int x, int y) east = (current.x + 1, current.y);

    if (NORTH_PIPES.Contains(currentType) && north.y >= 0 && (currentType != 'S' || SOUTH_PIPES.Contains(map[north.y][north.x])) && previous != north)
    {
        return north;
    }
    if (SOUTH_PIPES.Contains(currentType) && south.y < mapHeight && (currentType != 'S' || NORTH_PIPES.Contains(map[south.y][south.x])) && previous != south)
    {
        return south;
    }
    if (WEST_PIPES.Contains(currentType) && west.x >= 0 && (currentType != 'S' || EAST_PIPES.Contains(map[west.y][west.x])) && previous != west)
    {
        return west;
    }
    if (EAST_PIPES.Contains(currentType) && east.x < mapWidth && (currentType != 'S' || WEST_PIPES.Contains(map[east.y][east.x])) && previous != east)
    {
        return east;
    }

    throw new Exception("No path");
}

HashSet<(int x, int y)> FindLoop()
{
    (int x, int y) start = FindStart();

    HashSet<(int x, int y)> loop = [];
    (int x, int y)? previous = null;
    var current = start;
    do
    {
        var old = current;
        current = FindNextCell(previous, current);
        loop.Add(current);
        previous = old;
    } while (current != start);

    return loop;
}

string[] ComputeSimplifiedZoomedMap(HashSet<(int, int)> loop)
{
    // Compute a new map, twice as big as the original one, to handle the space between nodes.
    string[] zoomedMap = new string[zoomedMapHeight];

    for (int y = 0; y < zoomedMapHeight; y++)
    {
        string currentLine = "";
        for (int x = 0; x < zoomedMapWidth; x++)
        {
            if (x % 2 == 1 || y % 2 == 1)
            {
                // Newly inserted line, compute it.
                if (x % 2 == 1 && y % 2 == 1)
                {
                    // Don't compute diagonals, they are useless here.
                    currentLine += ' ';
                }
                else if (x % 2 == 1)
                {
                    // Merge between west and east nodes if required.
                    (int x, int y) west = ((x - 1) / 2, y / 2);
                    (int x, int y) east = ((x + 1) / 2, y / 2);
                    currentLine += loop.Contains(west) && loop.Contains(east) && EAST_PIPES.Contains(map[west.y][west.x]) ? '#' : ' ';
                }
                else
                {
                    // Merge between north and south nodes if required.
                    (int x, int y) north = (x / 2, (y - 1) / 2);
                    (int x, int y) south = (x / 2, (y + 1) / 2);
                    currentLine += loop.Contains(north) && loop.Contains(south) && SOUTH_PIPES.Contains(map[north.y][north.x]) ? '#' : ' ';
                }
            }
            else
            {
                // Existing line, it's either part of the loop or ground/junk.
                currentLine += loop.Contains((x / 2, y / 2)) ? '#' : '.';
            }
        }
        zoomedMap[y] = currentLine;
    }

    return zoomedMap;
}

bool IsInsideLoop(string[] zoomedMap, (int x, int y) cell, HashSet<(int x, int y)> visited)
{
    if (visited.Contains(cell))
    {
        return true;
    }

    if (cell.x < 0 || cell.x >= zoomedMapWidth || cell.y < 0 || cell.y >= zoomedMapHeight)
    {
        return false;
    }
    if (zoomedMap[cell.y][cell.x] == '#')
    {
        return true;
    }

    visited.Add(cell);

    return IsInsideLoop(zoomedMap, (cell.x, cell.y - 1), visited)
        && IsInsideLoop(zoomedMap, (cell.x, cell.y + 1), visited)
        && IsInsideLoop(zoomedMap, (cell.x - 1, cell.y), visited)
        && IsInsideLoop(zoomedMap, (cell.x + 1, cell.y), visited);
}

int CalculateArea(HashSet<(int, int)> loop)
{
    string[] zoomedMap = ComputeSimplifiedZoomedMap(loop);
    return Enumerable.Range(0, zoomedMapWidth)
        .SelectMany(x => Enumerable.Range(0, zoomedMapHeight).Select(y => (x, y)))
        .Where(cell => zoomedMap[cell.y][cell.x] == '.')
        .Where(cell => IsInsideLoop(zoomedMap, cell, []))
        .Count();
}

HashSet<(int, int)> loop = FindLoop();
Console.WriteLine(loop.Count / 2);
Console.WriteLine(CalculateArea(loop));
