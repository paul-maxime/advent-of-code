List<(string type, int value)> instructions = File.ReadAllLines("input")
    .Select(line => line.Split(" "))
    .Select(line => (line[0], line.Length > 1 ? int.Parse(line[1]) : 0))
    .ToList();

int cycle = 0;
int registerX = 1;
int strength = 0;

List<string> output = new List<string>();
string row = "";

void ProcessCycle()
{
    row += Math.Abs((cycle % 40) - registerX) <= 1 ? '#' : ' ';

    cycle += 1;
    if ((cycle % 40) == 20)
    {
        strength += cycle * registerX;
    }
    if ((cycle % 40) == 0)
    {
        output.Add(row);
        row = "";
    }
}

foreach (var instruction in instructions)
{
    switch (instruction.type)
    {
        case "addx":
            ProcessCycle();
            ProcessCycle();
            registerX += instruction.value;
            break;
        case "noop":
            ProcessCycle();
            break;
    }
}

Console.WriteLine(strength);
output.ForEach(line => Console.WriteLine(line));
