using System.Numerics;

var lines = File.ReadAllLines("input")
    .Select(line => line.Replace(" ", "").Replace("@", ",").Split(",").Select(BigInteger.Parse).Select(x => x * 1000).ToArray())
    .Select(line => new Line3D(
        new Point3D(line[0], line[1], line[2]),
        new Point3D(line[3], line[4], line[5])
    )).ToList();

// (BigInteger min, BigInteger max) TEST_AREA = (min: 7 * 1000, max: 24 * 1000);
(BigInteger min, BigInteger max) TEST_AREA = (min: 200000000000000 * 1000, max: 400000000000000 * 1000);

Point3D? IntersectLines2D(Line3D lineA, Line3D lineB)
{
    BigInteger x1 = lineA.Position.X;
    BigInteger y1 = lineA.Position.Y;
    BigInteger x2 = x1 + lineA.Velocity.X;
    BigInteger y2 = y1 + lineA.Velocity.Y;

    BigInteger x3 = lineB.Position.X;
    BigInteger y3 = lineB.Position.Y;
    BigInteger x4 = x3 + lineB.Velocity.X;
    BigInteger y4 = y3 + lineB.Velocity.Y;

    BigInteger numeratorX = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
    BigInteger numeratorY = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);
    BigInteger denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

    if (denominator == 0) return null;

    return new Point3D(numeratorX / denominator, numeratorY / denominator, 0);
}

int strikes = 0;

for (int i = 0; i < lines.Count - 1; i++)
{
    for (int j = i + 1; j < lines.Count; j++)
    {
        Point3D? intersection = IntersectLines2D(lines[i], lines[j]);

        if (intersection == null) continue;
        if (intersection.X < TEST_AREA.min || intersection.X > TEST_AREA.max) continue;
        if (intersection.Y < TEST_AREA.min || intersection.Y > TEST_AREA.max) continue;

        if (lines[i].Velocity.X < 0 && intersection.X > lines[i].Position.X) continue;
        if (lines[i].Velocity.Y < 0 && intersection.Y > lines[i].Position.Y) continue;
        if (lines[i].Velocity.X > 0 && intersection.X < lines[i].Position.X) continue;
        if (lines[i].Velocity.Y > 0 && intersection.Y < lines[i].Position.Y) continue;

        if (lines[j].Velocity.X < 0 && intersection.X > lines[j].Position.X) continue;
        if (lines[j].Velocity.Y < 0 && intersection.Y > lines[j].Position.Y) continue;
        if (lines[j].Velocity.X > 0 && intersection.X < lines[j].Position.X) continue;
        if (lines[j].Velocity.Y > 0 && intersection.Y < lines[j].Position.Y) continue;

        strikes += 1;
    }
}

Console.WriteLine(strikes);

record Point3D(BigInteger X, BigInteger Y, BigInteger Z);
record Line3D(Point3D Position, Point3D Velocity);
