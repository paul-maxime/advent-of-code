System.Text.RegularExpressions.Regex numberRegex = new("[0-9]+");

List<long> inputNumbers = numberRegex
    .Matches(File.ReadAllText("input"))
    .Select(match => long.Parse(match.Value))
    .ToList();

// Part 1

List<long> program = [];
List<long> output = [];
long registerA = 0;
long registerB = 0;
long registerC = 0;
long ip = 0;

void ResetProgram()
{
    program = inputNumbers.Skip(3).ToList();
    output = [];
    registerA = inputNumbers[0];
    registerB = inputNumbers[1];
    registerC = inputNumbers[2];
    ip = 0;
}

long GetComboOperand(long operand)
{
    if (operand >= 0 && operand <= 3) return operand;
    if (operand == 4) return registerA;
    if (operand == 5) return registerB;
    if (operand == 6) return registerC;
    throw new Exception($"Invalid combo operand: {operand}");
}

bool ExecuteTick()
{
    if (ip >= program.Count - 1) return false;

    long opcode = program[(int)ip];
    long operand = program[(int)ip + 1];

    if (opcode == 0)
    {
        registerA /= (int)Math.Pow(2, GetComboOperand(operand));
    }
    if (opcode == 1)
    {
        registerB ^= operand;
    }
    if (opcode == 2)
    {
        registerB = GetComboOperand(operand) % 8;
    }
    if (opcode == 3)
    {
        if (registerA != 0)
        {
            ip = operand - 2;
        }
    }
    if (opcode == 4)
    {
        registerB ^= registerC;
    }
    if (opcode == 5)
    {
        output.Add(GetComboOperand(operand) % 8);
    }
    if (opcode == 6)
    {
        registerB = registerA / (int)Math.Pow(2, GetComboOperand(operand));
    }
    if (opcode == 7)
    {
        registerC = registerA / (int)Math.Pow(2, GetComboOperand(operand));
    }

    ip += 2;
    return true;
}

void ExecuteProgram(long? a = null)
{
    ResetProgram();
    if (a.HasValue) registerA = a.Value;
    while (ExecuteTick()) {}
}

// Part 2

List<long> bruteforcing = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];

int CountValidDigitsFromStart(List<long> digits)
{
    for (int i = 0; i < program.Count; i++)
    {
        if (i >= digits.Count || program[i] != digits[i])
        {
            return i;
        }
    }
    return digits.Count;
}

long GenerateA()
{
    long a = 0;
    for (int i = 0; i < bruteforcing.Count; i++)
    {
        a |= bruteforcing[i] << ((bruteforcing.Count - i - 1) * 3);
    }
    return a;
}

void IncrementBruteforce(int startingAtIndex)
{
    int remainder = 1;
    int index = startingAtIndex;
    while (remainder > 0)
    {
        bruteforcing[index] += 1;
        remainder = 0;
        if (bruteforcing[index] == 8)
        {
            bruteforcing[index] = 0;
            remainder = 1;
            index -= 1;
        }
    }
}

long BruteforceSolution()
{
    int index = bruteforcing.Count - 1;
    for (int i = 0; i < 100000; i++) // 100000 should be enough for all inputs
    {
        IncrementBruteforce(index);

        long testedA = GenerateA();
        ExecuteProgram(testedA);

        List<long> digits = output;
        int validDigits = CountValidDigitsFromStart(digits);

        if (validDigits > 15)
        {
            return testedA;
        }

        // If the last n*3 bits are correct, we know our register is correct up to (n - 2)*3 bits.
        // This is what makes the bruteforce possible, we skip many values.
        int newIndex = bruteforcing.Count - (validDigits - 2);
        if (index > newIndex) index = newIndex;
    }

    return -1;
}

ExecuteProgram();
Console.WriteLine(string.Join(",", output));
Console.WriteLine(BruteforceSolution());
