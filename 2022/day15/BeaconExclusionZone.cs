var regex = new System.Text.RegularExpressions.Regex(@"x=(-?\d+), y=(-?\d+):.+x=(-?\d+), y=(-?\d+)");

var sensors = File.ReadAllLines("input")
    .Select(line => regex.Match(line))
    .Select(match => (
        x: int.Parse(match.Groups[1].Value),
        y: int.Parse(match.Groups[2].Value),
        bx: int.Parse(match.Groups[3].Value),
        by: int.Parse(match.Groups[4].Value)
    ))
    .Select(sensor => (
        sensor.x, sensor.y, sensor.bx, sensor.by,
        r: Math.Abs(sensor.x - sensor.bx) + Math.Abs(sensor.y - sensor.by)
    ))
    .ToList();

var beacons = sensors.Select(sensor => (x: sensor.bx, y: sensor.by)).Distinct().ToList();

int maxRange = sensors.Max(sensor => sensor.r);
int minX = sensors.Min(sensor => Math.Min(sensor.x, sensor.bx)) - maxRange;
int maxX = sensors.Max(sensor => Math.Max(sensor.x, sensor.bx)) + maxRange;

void InspectRow(int y, int fromX, int toX, bool isPart2)
{
    int total = 0;
    int x = fromX;
    while (x <= toX)
    {
        bool found = false;
        foreach (var sensor in sensors)
        {
            int distanceFromSensor = Math.Abs(sensor.x - x) + Math.Abs(sensor.y - y);
            if (distanceFromSensor <= sensor.r)
            {
                int sensorMinX = sensor.x - sensor.r + Math.Abs(sensor.y - y);
                int sensorMaxX = sensor.x + sensor.r - Math.Abs(sensor.y - y);

                int remaining = sensorMaxX - x + 1;

                if (!isPart2)
                {
                    int beaconsInLine = beacons.Where(b => b.y == y && b.x >= x && b.x <= x + remaining).Count();
                    total += remaining - beaconsInLine;
                }

                x += remaining;
                found = true;
                break;
            }
        }
        if (!found)
        {
            if (isPart2 && x >= 0 && y >= 0 && x <= 4000000 && y <= 4000000)
            {
                Console.WriteLine(x * 4000000L + y);
                break;
            }
            x += 1;
        }
    }
    if (!isPart2)
    {
        Console.WriteLine(total);
    }
}

InspectRow(2000000, minX, maxX, false);
for (int y = 0; y <= 4000000; ++y)
{
    InspectRow(y, 0, 4000000, true);
}
