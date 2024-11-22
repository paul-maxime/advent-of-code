using System.Numerics;

var lines = File.ReadAllLines("input")
    .Select(line => line.Replace(" ", "").Replace("@", ",").Split(",").Select(BigInteger.Parse).ToArray())
    .Select(line => new Line3D(
        new Point3D(line[0], line[1], line[2]),
        new Point3D(line[3], line[4], line[5])
    )).ToList();


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

void SolveIn2D()
{
    // (BigInteger min, BigInteger max) TEST_AREA = (min: 7, max: 24);
    (BigInteger min, BigInteger max) TEST_AREA = (min: 200000000000000, max: 400000000000000);

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
}

void SolveIn3D()
{
    var context = new Microsoft.Z3.Context();
    var solver = context.MkSolver();

    var rockPositionX = context.MkIntConst("x");
    var rockPositionY = context.MkIntConst("y");
    var rockPositionZ = context.MkIntConst("z");

    var rockVelocityX = context.MkIntConst("vx");
    var rockVelocityY = context.MkIntConst("vy");
    var rockVelocityZ = context.MkIntConst("vz");

    for (int i = 0; i < lines.Count; i++)
    {
        var line = lines[i];

        var linePositionX = context.MkInt(line.Position.X.ToString());
        var linePositionY = context.MkInt(line.Position.Y.ToString());
        var linePositionZ = context.MkInt(line.Position.Z.ToString());

        var lineVelocityX = context.MkInt(line.Velocity.X.ToString());
        var lineVelocityY = context.MkInt(line.Velocity.Y.ToString());
        var lineVelocityZ = context.MkInt(line.Velocity.Z.ToString());

        var time = context.MkIntConst("t" + i);
        solver.Add(time >= 0);
        solver.Add(context.MkEq(context.MkAdd(linePositionX, context.MkMul(lineVelocityX, time)), context.MkAdd(rockPositionX + context.MkMul(rockVelocityX, time))));
        solver.Add(context.MkEq(context.MkAdd(linePositionY, context.MkMul(lineVelocityY, time)), context.MkAdd(rockPositionY + context.MkMul(rockVelocityY, time))));
        solver.Add(context.MkEq(context.MkAdd(linePositionZ, context.MkMul(lineVelocityZ, time)), context.MkAdd(rockPositionZ + context.MkMul(rockVelocityZ, time))));
    }

    if (solver.Check() != Microsoft.Z3.Status.SATISFIABLE)
    {
        Console.WriteLine("Unsatisfiable conditions?");
        return;
    }

    var model = solver.Model;
    var resultPosX = model.Eval(rockPositionX);
    var resultPosY = model.Eval(rockPositionY);
    var resultPosZ = model.Eval(rockPositionZ);
    var resultVelocityX = model.Eval(rockVelocityX);
    var resultVelocityY = model.Eval(rockVelocityY);
    var resultVelocityZ = model.Eval(rockVelocityZ);

    Console.WriteLine($"({resultPosX}, {resultPosY}, {resultPosZ}) @ {resultVelocityX}, {resultVelocityY}, {resultVelocityZ}");
    Console.WriteLine(BigInteger.Parse(resultPosX.ToString()) + BigInteger.Parse(resultPosY.ToString()) + BigInteger.Parse(resultPosZ.ToString()));
}

SolveIn2D();
SolveIn3D();

record Point3D(BigInteger X, BigInteger Y, BigInteger Z);
record Line3D(Point3D Position, Point3D Velocity);
