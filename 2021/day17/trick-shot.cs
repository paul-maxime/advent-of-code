using System.Text.RegularExpressions;

int[] input = new Regex(@"(-?[0-9]+)")
    .Matches(File.ReadAllText("input"))
    .Select(x => x.Captures[0].Value)
    .Select(int.Parse)
    .ToArray();

(int fromX, int toX, int fromY, int toY) = (input[0], input[1], input[2], input[3]);

(bool success, int height) LaunchProbe((int x, int y) velocity)
{
    int height = velocity.y > 0 ? velocity.y * (velocity.y + 1) / 2 : 0;

    (int x, int y) pos = (0, 0);
    while (velocity.x != 0 || pos.y > toY)
    {
        pos.x += velocity.x;
        pos.y += velocity.y;
        if (pos.x >= fromX && pos.x <= toX && pos.y >= fromY && pos.y <= toY)
        {
            return (true, height);
        }
        velocity.x += -Math.Sign(velocity.x);
        velocity.y -= 1;
    }
    return (false, 0);
}

int[] heights = Enumerable.Range(0, toX + 1)
    .SelectMany(x => Enumerable.Range(fromY, -fromY * 2 + 1).Select(y => (x, y)))
    .Select(LaunchProbe)
    .Where(result => result.success)
    .Select(result => result.height)
    .ToArray();

Console.WriteLine(heights.Max());
Console.WriteLine(heights.Count());
