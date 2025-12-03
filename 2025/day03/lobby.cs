string[] input = File.ReadAllLines("input");

string FindLargestBatteries(string bank, int remaining)
{
    if (remaining == 0) return "";
    char first = bank[..^(remaining - 1)].Max();
    int firstIndex = bank.IndexOf(first);
    return first + FindLargestBatteries(bank[(firstIndex + 1)..], remaining - 1);
}

Console.WriteLine(input.Select(x => FindLargestBatteries(x, 2)).Select(long.Parse).Sum());
Console.WriteLine(input.Select(x => FindLargestBatteries(x, 12)).Select(long.Parse).Sum());
