string[] input = File.ReadAllText("input").Split("\n\n");

Dictionary<string, bool> wires = input[0]
    .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(line => line.Split(": "))
    .ToDictionary(line => line[0], line => line[1] == "1");

Dictionary<string, (string a, string op, string b)> gates = input[1]
    .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(line => line.Split(" "))
    .ToDictionary(line => line[4], line => (line[0], line[1], line[2]));

List<string> allWires = wires.Keys.Concat(gates.Keys).OrderDescending().ToList();

Random random = new();
Dictionary<string, string> swaps = [];

bool GetWire(string wire, int depth = 0)
{
    if (depth == 100)
    {
        throw new Exception("loop");
    }
    if (swaps.TryGetValue(wire, out var swappedWire))
    {
        wire = swappedWire;
    }
    if (wires.TryGetValue(wire, out bool value))
    {
        return value;
    }
    if (gates.TryGetValue(wire, out var gate))
    {
        if (gate.op == "AND") return GetWire(gate.a, depth + 1) && GetWire(gate.b, depth + 1);
        if (gate.op == "OR") return GetWire(gate.a, depth + 1) || GetWire(gate.b, depth + 1);
        if (gate.op == "XOR") return GetWire(gate.a, depth + 1) != GetWire(gate.b, depth + 1);
        throw new Exception("Invalid operation " + gate.op);
    }
    throw new Exception("Invalid wire " + wire);
}

long GetNumber(char header)
{
    long output = 0;
    foreach (string gate in allWires.Where(x => x.StartsWith(header)))
    {
        output <<= 1;
        if (GetWire(gate)) output += 1;
    }
    return output;
}

void PrintGraphviz()
{
    Dictionary<string, string> opToColor = new()
    {
        ["AND"] = "red",
        ["OR"] = "green",
        ["XOR"] = "blue",
    };
    Console.WriteLine("digraph {");
    foreach (var gate in gates)
    {
        Console.WriteLine(gate.Value.a + " -> " + gate.Key + " [color=" + opToColor[gate.Value.op] + "]");
        Console.WriteLine(gate.Value.b + " -> " + gate.Key + " [color=" + opToColor[gate.Value.op] + "]");
    }
    Console.WriteLine("}");
}

void RandomizeWires()
{
    foreach (string wire in wires.Keys)
    {
        wires[wire] = random.NextDouble() >= 0.5;
    }
}

string FindSwappedGates()
{
    // Use the graph to determine what to swap.
    swaps.Add("gwh", "z09");
    swaps.Add("z09", "gwh");

    swaps.Add("wgb", "wbw");
    swaps.Add("wbw", "wgb");

    swaps.Add("z21", "rcb");
    swaps.Add("rcb", "z21");

    swaps.Add("jct", "z39");
    swaps.Add("z39", "jct");

    // Test with 100 different initial values to make sure we're always correct.
    for (int i = 0; i < 100; i++)
    {
        RandomizeWires();
        long sum = GetNumber('x') + GetNumber('y');
        long expected = GetNumber('z');
        if (sum != expected)
        {
            // Something went wrong, our sum isn't correct.
            // Print binary values to determine which bit is wrong.
            Console.WriteLine(Convert.ToString(sum, 2));
            Console.WriteLine(Convert.ToString(expected, 2));
            return "error";
        }
    }
    return string.Join(",", swaps.Keys.Order());
}

PrintGraphviz();
Console.WriteLine(GetNumber('z'));
Console.WriteLine(FindSwappedGates());
