using System.Text.RegularExpressions;

Regex regex = new Regex(@"-?[0-9]+");

List<(bool on, Box3D box)> input = File.ReadAllLines("input")
    .Select(line => (line[1] == 'n', regex.Matches(line)))
    .Select(matches => (matches.Item1, matches.Item2.Select(match => long.Parse(match.Value)).ToArray()))
    .Select(x => (x.Item1, new Box3D(x.Item2)))
    .ToList();

List<Box3D> cubes = new();
bool initializing = true;

foreach (var step in input)
{
    if (initializing && (
        step.box.fromX < -50 || step.box.toX > 50 ||
        step.box.fromY < -50 || step.box.toY > 50 ||
        step.box.fromZ < -50 || step.box.toZ > 50))
    {
        initializing = false;
        Console.WriteLine("Initialization: " + cubes.Select(x => x.Volume).Sum());
    }

    IntersectNewCube(step.box, !step.on);
}

Console.WriteLine("Full reboot: " + cubes.Select(x => x.Volume).Sum());

void IntersectNewCube(Box3D newBox, bool removal = false)
{
    foreach (Box3D existingBox in cubes)
    {
        if (!newBox.Intersect(existingBox)) continue;

        cubes.Remove(existingBox);
        foreach (Box3D subNewBox in newBox.GetIntersectionsWith(existingBox))
        {
            if (subNewBox.Intersect(newBox))
            {
                IntersectNewCube(subNewBox, removal);
            }
            else
            {
                cubes.Add(subNewBox);
            }
        }
        return;
    }
    if (!removal)
    {
        cubes.Add(newBox);
    }
}

class Box3D
{
    public long fromX { get; private set; }
    public long toX { get; private set; }
    public long fromY { get; private set; }
    public long toY { get; private set; }
    public long fromZ { get; private set; }
    public long toZ { get; private set; }

    public long Volume => (toX - fromX + 1) * (toY - fromY + 1) * (toZ - fromZ + 1);

    public Box3D(long fromX, long toX, long fromY, long toY, long fromZ, long toZ)
    {
        this.fromX = fromX;
        this.toX = toX;
        this.fromY = fromY;
        this.toY = toY;
        this.fromZ = fromZ;
        this.toZ = toZ;
    }

    public Box3D(long[] array)
    {
        fromX = array[0];
        toX = array[1];
        fromY = array[2];
        toY = array[3];
        fromZ = array[4];
        toZ = array[5];
    }

    public bool Intersect(Box3D boxB) =>
        this.fromX < boxB.toX + 1 && this.toX + 1 > boxB.fromX &&
        this.fromY < boxB.toY + 1 && this.toY + 1 > boxB.fromY &&
        this.fromZ < boxB.toZ + 1 && this.toZ + 1 > boxB.fromZ;

    public IEnumerable<Box3D> GetIntersectionsWith(Box3D boxB)
    {
        Box3D boxA = this;

        List<(long from, long to)> xAxis = new() {
            (Math.Min(boxA.fromX, boxB.fromX), Math.Max(boxA.fromX, boxB.fromX) - 1),
            (Math.Max(boxA.fromX, boxB.fromX), Math.Min(boxA.toX, boxB.toX)),
            (Math.Min(boxA.toX, boxB.toX) + 1, Math.Max(boxA.toX, boxB.toX)),
        };
        List<(long from, long to)> yAxis = new() {
            (Math.Min(boxA.fromY, boxB.fromY), Math.Max(boxA.fromY, boxB.fromY) - 1),
            (Math.Max(boxA.fromY, boxB.fromY), Math.Min(boxA.toY, boxB.toY)),
            (Math.Min(boxA.toY, boxB.toY) + 1, Math.Max(boxA.toY, boxB.toY)),
        };
        List<(long from, long to)> zAxis = new() {
            (Math.Min(boxA.fromZ, boxB.fromZ), Math.Max(boxA.fromZ, boxB.fromZ) - 1),
            (Math.Max(boxA.fromZ, boxB.fromZ), Math.Min(boxA.toZ, boxB.toZ)),
            (Math.Min(boxA.toZ, boxB.toZ) + 1, Math.Max(boxA.toZ, boxB.toZ)),
        };

        foreach (var x in xAxis)
        foreach (var y in yAxis)
        foreach (var z in zAxis)
        {
            Box3D box = new Box3D(x.from, x.to, y.from, y.to, z.from, z.to);
            if (box.Intersect(boxA) || box.Intersect(boxB))
            {
                yield return box;
            }
        }
    }
}
