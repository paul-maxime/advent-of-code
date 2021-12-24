List<string[]> instructions = File.ReadAllLines("input")
    .Select(line => line.Split(" "))
    .ToList();

string greatest = BruteforceEverything(instructions, false);
string smallest = BruteforceEverything(instructions, true);

Console.WriteLine(greatest);
Console.WriteLine(smallest);

string BruteforceEverything(List<string[]> instructions, bool smallest)
{
    Dictionary<int, string> allowed = new() { {0, ""} };

    int blockStart = instructions.Count;
    while (true)
    {
        blockStart = instructions.FindLastIndex(blockStart - 1, x => x[0] == "inp");

        Console.WriteLine($"Bruteforcing block {blockStart}, {allowed.Count} possible Z inputs");
        allowed = BruteforceDigit(instructions, allowed, blockStart, smallest);

        if (blockStart == 0) break;
    }

    return allowed.First().Value;
}

Dictionary<int, string> BruteforceDigit(List<string[]> instructions, Dictionary<int, string> allowed, int blockIndex, bool smallest)
{
    Dictionary<int, string> bruteforced = new();
    int w = smallest ? 1 : 9;
    while (true)
    {
        for (int z = -10000; z <= 10000; z += 1)
        {
            Dictionary<string, int> registers = new()
            {
                {"w", w},
                {"x", 0},
                {"y", 0},
                {"z", z},
            };

            ExecuteBlock(instructions, registers, blockIndex + 1);

            if (allowed.ContainsKey(registers["z"]) && !bruteforced.ContainsKey(z))
            {
                bruteforced.Add(z, w + allowed[registers["z"]]);
            }
        }
        w += smallest ? 1 : -1;
        if (w < 1 || w > 9) break;
    }
    return bruteforced;
}

void ExecuteBlock(List<string[]> instructions, Dictionary<string, int> registers, int ip)
{
    while (true)
    {
        if (ip == instructions.Count || instructions[ip][0] == "inp") break;
        Execute(registers, instructions[ip]);
        ip++;
    }
}

void Execute(Dictionary<string, int> registers, string[] instruction)
{
    int right = instruction.Length == 3 ? registers.ContainsKey(instruction[2]) ? registers[instruction[2]] : int.Parse(instruction[2]) : 0;
    switch (instruction[0])
    {
        case "add":
            registers[instruction[1]] += right;
            break;
        case "mul":
            registers[instruction[1]] *= right;
            break;
        case "div":
            registers[instruction[1]] /= right;
            break;
        case "mod":
            registers[instruction[1]] %= right;
            break;
        case "eql":
            registers[instruction[1]] = (registers[instruction[1]] == right) ? 1 : 0;
            break;
    }
}
