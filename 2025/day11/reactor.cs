Dictionary<string, string[]> devices = File.ReadAllLines("input")
    .Select(line => line.Split(": "))
    .Select(data => (
        data[0],
        data[1].Split(" ")
    )).ToDictionary();

Dictionary<(string, bool, bool), long> cache = [];

long Traverse(string device, bool dac, bool fft)
{
    if (device == "out") return dac && fft ? 1 : 0;

    if (cache.TryGetValue((device, dac, fft), out long result))
    {
        return result;
    }

    long count = 0;
    foreach (string output in devices[device])
    {
        count += Traverse(output, device == "dac" || dac, device == "fft" || fft);
    }

    cache.Add((device, dac, fft), count);
    return count;
}

Console.WriteLine(Traverse("you", true, true));
Console.WriteLine(Traverse("svr", false, false));
