int[] input = File.ReadAllLines("input").Select(x => int.Parse(x)).ToArray();

int GetDepthIncreases(int[] input)
{
    int increases = 0;
    for (int i = 1; i < input.Length; i++)
    {
        if (input[i] > input[i - 1]) increases++;
    }
    return increases;
}

int GetThreeMeasurementIncreases(int[] input)
{
    int increases = 0;
    for (int i = 0; i < input.Length - 3; i++)
    {
        int sumA = input[i] + input[i + 1] + input[i + 2];
        int sumB = input[i + 1] + input[i + 2] + input[i + 3];
        if (sumB > sumA) increases++;
    }
    return increases;
}

Console.WriteLine($"Increased {GetDepthIncreases(input)} times");
Console.WriteLine($"Increased {GetThreeMeasurementIncreases(input)} times (3-measurements)");
