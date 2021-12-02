Movement[] input = File.ReadAllLines("input")
    .Select(x => x.Split(" "))
    .Select(x => new Movement { direction = x[0], distance = int.Parse(x[1]) })
    .ToArray();

void ComputeDepthAndPosition(Movement[] input)
{
    int x = 0;
    int y = 0;

    foreach (var movement in input)
    {
        switch (movement.direction)
        {
            case "forward":
                x += movement.distance;
                break;
            case "up":
                y -= movement.distance;
                break;
            case "down":
                y += movement.distance;
                break;
        }
    }

    Console.WriteLine($"X: {x}, Y: {y}, Result: {x * y}");
}

void ComputeUsingAim(Movement[] input)
{
    int aim = 0;
    int position = 0;
    int depth = 0;

    foreach (var movement in input)
    {
        switch (movement.direction)
        {
            case "forward":
                position += movement.distance;
                depth += aim * movement.distance;
                break;
            case "up":
                aim -= movement.distance;
                break;
            case "down":
                aim += movement.distance;
                break;
        }
    }

    Console.WriteLine($"Position: {position}, Depth: {depth}, Result: {position * depth}");
}

ComputeDepthAndPosition(input);
ComputeUsingAim(input);

class Movement
{
    public string direction = "";
    public int distance = 0;
}
