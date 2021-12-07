int[] crabs = File.ReadAllText("input").Split(",").Select(int.Parse).ToArray();

long ComputeCost(int[] crabs, int destination)
{
    return crabs.Select(x => Math.Abs(destination - x)).Sum();
}

long ComputeTriangularCost(int[] crabs, int destination)
{
    return crabs.Select(x => Math.Abs(destination - x)).Select(x => x * (x + 1) / 2).Sum();
}

long minCost = Enumerable.Range(crabs.Min(), crabs.Max()).Select(x => ComputeCost(crabs, x)).Min();
long triangularMinCost = Enumerable.Range(crabs.Min(), crabs.Max()).Select(x => ComputeTriangularCost(crabs, x)).Min();

Console.WriteLine($"Cost: {minCost}");
Console.WriteLine($"Triangular cost: {triangularMinCost}");
