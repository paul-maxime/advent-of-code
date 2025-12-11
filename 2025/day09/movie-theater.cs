List<(long x, long y)> input = File.ReadAllLines("input")
    .Select(line => line.Split(","))
    .Select(line => (long.Parse(line[0]), long.Parse(line[1])))
    .ToList();

List<((long x, long y) p1, (long x, long y) p2, long area)> rectangles = [];

for (int i = 0; i < input.Count; i++)
{
    for (int j = i + 1; j < input.Count; j++)
    {
        var a = input[i];
        var b = input[j];
        long area = (Math.Abs(a.x - b.x) + 1) * (Math.Abs(a.y - b.y) + 1);
        rectangles.Add((a, b, area));
    }
}

rectangles = rectangles.OrderByDescending(x => x.area).ToList();

Console.WriteLine(rectangles[0]);

Dictionary<long, List<(long from, long to)>> verticalLines = [];
Dictionary<long, List<(long from, long to)>> horizontalLines = [];

(long x, long y) previous = input[^1];
foreach ((long x, long y) current in input)
{
    long minX = Math.Min(current.x, previous.x);
    long minY = Math.Min(current.y, previous.y);
    long maxX = Math.Max(current.x, previous.x);
    long maxY = Math.Max(current.y, previous.y);

    if (minX == maxX)
    {
        if (!verticalLines.ContainsKey(minX)) verticalLines.Add(minX, []);
        verticalLines[minX].Add((minY, maxY));
    }

    if (minY == maxY)
    {
        if (!horizontalLines.ContainsKey(minY)) horizontalLines.Add(minY, []);
        horizontalLines[minY].Add((minX, maxX));
    }

    previous = current;
}

bool IsOnPolygonLineVertical((long x, long y) point)
{
    if (!verticalLines.TryGetValue(point.x, out List<(long from, long to)>? lines)) return false;
    return lines.Any(line => point.y > line.from && point.y <= line.to);
}

bool IsOnPolygonLineHorizontal((long x, long y) point)
{
    if (!horizontalLines.TryGetValue(point.y, out List<(long from, long to)>? lines)) return false;
    return lines.Any(line => point.x > line.from && point.x <= line.to);
}

bool IsOnPolygonLine((long x, long y) point)
{
    (long x, long y) previous = input[^1];
    foreach ((long x, long y) current in input)
    {
        long minX = Math.Min(current.x, previous.x);
        long minY = Math.Min(current.y, previous.y);
        long maxX = Math.Max(current.x, previous.x);
        long maxY = Math.Max(current.y, previous.y);
        if (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY)
        {
            return true;
        }
        previous = current;
    }
    return false;
}

bool IsInsidePolygonXFromTo(long fromX, long toX, long pointY)
{
    bool isInside = false;
    for (long x = 0; x <= toX; x++)
    {
        if (IsOnPolygonLineVertical((x, pointY)))
        {
            isInside = !isInside;
        }
        if (!isInside && x >= fromX && !IsOnPolygonLine((x, pointY)))
        {
            return false;
        }
    }
    return true;
}

bool IsInsidePolygonYFromTo(long fromY, long toY, long pointX)
{
    bool isInside = false;
    for (long y = 0; y <= toY; y++)
    {
        if (IsOnPolygonLineHorizontal((pointX, y)))
        {
            isInside = !isInside;
        }
        if (!isInside && y >= fromY && !IsOnPolygonLine((pointX, y)))
        {
            return false;
        }
    }
    return true;
}

int count = 0;
foreach (var rectangle in rectangles)
{
    count++;
    Console.WriteLine($"Checking {count} / {rectangles.Count}");
    long fromX = Math.Min(rectangle.p1.x, rectangle.p2.x);
    long fromY = Math.Min(rectangle.p1.y, rectangle.p2.y);
    long toX = Math.Max(rectangle.p1.x, rectangle.p2.x);
    long toY = Math.Max(rectangle.p1.y, rectangle.p2.y);

    if (!IsInsidePolygonXFromTo(fromX, toX, fromY)) continue;
    if (!IsInsidePolygonXFromTo(fromX, toX, toY)) continue;

    if (!IsInsidePolygonYFromTo(fromY, toY, fromX)) continue;
    if (!IsInsidePolygonYFromTo(fromY, toY, toX)) continue;

    Console.WriteLine(rectangle);
    break;
}
