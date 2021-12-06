int[] input = File.ReadAllText("input").Split(",").Select(int.Parse).ToArray();

Dictionary<int, long> InitializeFishes(int[] input)
{
    var fishes = new Dictionary<int, long>();
    for (int i = 0; i <= 8; i++)
    {
        fishes[i] = 0;
    }
    foreach (int fish in input)
    {
        fishes[fish]++;
    }
    return fishes;
}

void ProcessTurn(Dictionary<int, long> fishes)
{
    long eggs = fishes[0];
    for (int i = 0; i < 8; i++)
    {
        fishes[i] = fishes[i + 1];
    }
    fishes[6] += eggs;
    fishes[8] = eggs;
}

var fishes = InitializeFishes(input);

for (int turn = 1; turn <= 256; turn++)
{
    ProcessTurn(fishes);

    if (turn == 80 || turn == 256) {
        Console.WriteLine($"After turn {turn}: {fishes.Values.Sum()} fishes");
    }
}
