int[] input = File.ReadAllLines("input")
    .Select(line => int.Parse(line[1..]) * (line[0] == 'R' ? 1 : -1))
    .ToArray();

int ComputeZeros()
{
    int dial = 50;
    int zeros = 0;

    foreach (int rotation in input)
    {
        dial = (dial + rotation) % 100;
        if (dial == 0) zeros += 1;
    }
    return zeros;
}

int ComputeAllZeros()
{
    int dial = 50;
    int zeros = 0;

    foreach (int rotation in input)
    {
        for (int i = 0; i < Math.Abs(rotation); i++)
        {
            dial = (dial + Math.Sign(rotation)) % 100;
            if (dial == 0) zeros += 1;
        }
    }
    return zeros;
}


Console.WriteLine(ComputeZeros());
Console.WriteLine(ComputeAllZeros());
