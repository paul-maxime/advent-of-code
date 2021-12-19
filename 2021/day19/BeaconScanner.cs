List<Point3D[]> scanners = File.ReadAllText("input")
    .Split("\n\n")
    .Select(block => block
        .Split("\n")
        .Where(line => line.Contains(","))
        .Select(line => line.Split(",").Select(int.Parse).ToArray())
        .Select(point => new Point3D(point[0], point[1], point[2])).ToArray()
    ).ToList();

List<Point3D> BuildAvailableRotations()
{
    List<Point3D> availableRotations = new();
    HashSet<Point3D> seen = new();
    Point3D point = new Point3D(1, 2, 3);
    for (int rx = 0; rx < 360; rx += 90)
    {
        for (int ry = 0; ry < 360; ry += 90)
        {
            for (int rz = 0; rz < 360; rz += 90)
            {
                if (seen.Add(point.RotateX(rx).RotateY(ry).RotateZ(rz)))
                {
                    availableRotations.Add(new Point3D(rx, ry, rz));
                }
            }
        }
    }
    return availableRotations;
}

List<Point3D> availableRotations = BuildAvailableRotations();

(Point3D delta, Point3D[] normalized)? OverlapAndNormalize(Point3D[] scannerA, Point3D[] scannerB)
{
    foreach (Point3D rotation in availableRotations)
    {
        HashSet<Point3D> rotatedB = scannerB.Select(x => x.Rotate(rotation)).ToHashSet();
        foreach (Point3D pointA in scannerA)
        {
            foreach (Point3D pointB in rotatedB)
            {
                Point3D delta = pointA - pointB;
                int match = scannerA.Select(x => x - delta).Where(x => rotatedB.Contains(x)).Count();
                if (match >= 12)
                {
                    return (delta, rotatedB.Select(x => x + delta).ToArray());
                }
            }
        }
    }
    return null;
}

List<Point3D[]> remaining = scanners.Skip(1).ToList();
List<Point3D[]> normalized = scanners.Take(1).ToList();
List<Point3D> positions = new List<Point3D> { new Point3D(0, 0, 0) };

while (remaining.Count > 0)
{
    foreach (Point3D[] scannerA in normalized.ToList())
    {
        foreach (Point3D[] scannerB in remaining.ToList())
        {
            var result = OverlapAndNormalize(scannerA, scannerB);
            if (result == null) continue;

            remaining.Remove(scannerB);
            normalized.Add(result.Value.normalized);
            positions.Add(result.Value.delta);

            Console.WriteLine($"scanner {result.Value.delta} found, remaining: {remaining.Count}");
        }
    }
}

Console.WriteLine(normalized.SelectMany(x => x).ToHashSet().Count);
Console.WriteLine(positions.SelectMany(a => positions.Select(b => a.ManhattanDistanceTo(b))).Max());
