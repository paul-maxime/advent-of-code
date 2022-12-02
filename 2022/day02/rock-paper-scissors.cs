List<(char column1, char column2)> strategy = File.ReadAllLines("input")
    .Select(line => (line[0], line[2]))
    .ToList();

Shape ShapeFromChar(char c) => c switch {
    'A' or 'X' => Shape.Rock,
    'B' or 'Y' => Shape.Paper,
    'C' or 'Z' => Shape.Scissors,
    _ => throw new Exception($"Invalid shape {c}"),
};

Shape ShapeFromResult(Shape other, char result) => result switch {
    'X' => (Shape)(((int)other + 2) % 3),
    'Y' => other,
    'Z' => (Shape)(((int)other + 1) % 3),
    _ => throw new Exception($"Invalid result {result}"),
};

int GetRPSResult(Shape shapeA, Shape shapeB)
{
    if (shapeA == shapeB) return 0;
    if ((int)shapeA == ((int)shapeB + 1) % 3) return 1;
    return 2;
}

int GetShapeScore(Shape shape) => (int)shape + 1;

int GetWinningScore(int winner) => winner switch {
    0 => 3, // draw
    1 => 0, // lost
    2 => 6, // won
    _ => throw new Exception($"Invalid winner {winner}"),
};

int GetStrategyScore(bool correctVersion) => strategy.Select(round => {
    Shape shapeA = ShapeFromChar(round.column1);
    Shape shapeB = correctVersion ? ShapeFromResult(shapeA, round.column2) : ShapeFromChar(round.column2);
    return GetWinningScore(GetRPSResult(shapeA, shapeB)) + GetShapeScore(shapeB);
}).Sum();

Console.WriteLine(GetStrategyScore(false));
Console.WriteLine(GetStrategyScore(true));

enum Shape { Rock = 0, Paper = 1, Scissors = 2 }
