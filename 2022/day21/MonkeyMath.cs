Dictionary<string, Monkey> monkeys = File.ReadAllLines("input")
    .Select(line => new Monkey(line))
    .ToDictionary(x => x.name);

long GetNumber(string name)
{
    Monkey monkey = monkeys[name];
    if (monkey.immediate != 0) return monkey.immediate;
    long left = GetNumber(monkey.left);
    long right = GetNumber(monkey.right);
    switch (monkey.operation)
    {
        case '+': return left + right;
        case '-': return left - right;
        case '*': return left * right;
        case '/': return left / right;
    }
    throw new Exception("Invalid operation");
}

long ComputeDelta()
{
    Monkey root = monkeys["root"];
    long left = GetNumber(root.left);
    long right = GetNumber(root.right);
    return left - right;
}

long FindHumanNumber()
{
    Monkey human = monkeys["humn"];
    long minValue = 1;
    long maxValue = 1000000000000000;
    while (true)
    {
        long currentValue = (minValue + maxValue) / 2;
        human.immediate = currentValue;
        long delta = ComputeDelta();
        Console.WriteLine($"Trying {currentValue} -> {delta}");

        if (delta > 0) {
            minValue = currentValue;
        } else if (delta < 0) {
            maxValue = currentValue;
        } else {
            return currentValue;
        }
    }
}

Console.WriteLine(GetNumber("root"));
Console.WriteLine(FindHumanNumber());

class Monkey
{
    public string name;
    public string left = "";
    public string right = "";
    public char operation = '\0';
    public long immediate = 0;

    public Monkey(string line)
    {
        string[] data = line.Split(": ");
        this.name = data[0];

        string[] operation = data[1].Split(" ");
        if (operation.Length == 1)
        {
            this.immediate = int.Parse(operation[0]);
        }
        else
        {
            this.left = operation[0];
            this.operation = operation[1][0];
            this.right = operation[2];
        }
    }
}