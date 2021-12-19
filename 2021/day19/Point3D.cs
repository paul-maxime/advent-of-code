public struct Point3D
{
    public int X, Y, Z;

    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3D Rotate(Point3D euler)
    {
        return RotateX(euler.X).RotateY(euler.Y).RotateZ(euler.Z);
    }

    public Point3D RotateX(int angle)
    {
        switch (angle)
        {
            case 0:
                return new Point3D(X, Y, Z);
            case 90:
                return new Point3D(X, -Z, Y);
            case 180:
                return new Point3D(X, -Y, -Z);
            case 270:
                return new Point3D(X, Z, -Y);
        }
        throw new Exception("Unsupported angle");
    }

    public Point3D RotateY(int angle)
    {
        switch (angle)
        {
            case 0:
                return new Point3D(X, Y, Z);
            case 90:
                return new Point3D(Z, Y, -X);
            case 180:
                return new Point3D(-X, Y, -Z);
            case 270:
                return new Point3D(-Z, Y, X);
        }
        throw new Exception("Unsupported angle");
    }

    public Point3D RotateZ(int angle)
    {
        switch (angle)
        {
            case 0:
                return new Point3D(X, Y, Z);
            case 90:
                return new Point3D(-Y, X, Z);
            case 180:
                return new Point3D(-X, -Y, Z);
            case 270:
                return new Point3D(Y, -X, Z);
        }
        throw new Exception("Unsupported angle");
    }

    public int ManhattanDistanceTo(Point3D point)
    {
        return Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public static bool operator ==(Point3D pointA, Point3D pointB)
    {
        return pointA.Equals(pointB);
    }

    public static bool operator !=(Point3D pointA, Point3D pointB)
    {
        return !(pointA == pointB);
    }

    public static Point3D operator+(Point3D left, Point3D right)
    {
        return new Point3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Point3D operator-(Point3D left, Point3D right)
    {
        return new Point3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point3D point)
        {
            return point.X == X && point.Y == Y && point.Z == Z;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (X, Y, Z).GetHashCode();
    }
}
