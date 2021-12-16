(int version, long result) ReadPacket(string input, ref int index)
{
    int packetVersion = ReadInt(input, ref index, 3);
    int packetTypeId = ReadInt(input, ref index, 3);

    if (packetTypeId == 4)
    {
        long immediate = ReadImmediateValue(input, ref index);
        return (packetVersion, immediate);
    }

    List<long> subValues = new List<long>();

    int lengthTypeId = ReadInt(input, ref index, 1);
    if (lengthTypeId == 0)
    {
        int subLength = ReadInt(input, ref index, 15);
        int expected = index + subLength;
        while (index < expected)
        {
            (int subVersion, long subResult) = ReadPacket(input, ref index);
            packetVersion += subVersion;
            subValues.Add(subResult);
        }
    }
    else
    {
        int subPackets = ReadInt(input, ref index, 11);
        while (subPackets --> 0)
        {
            (int subVersion, long subResult) = ReadPacket(input, ref index);
            packetVersion += subVersion;
            subValues.Add(subResult);
        }
    }

    long result = ComputeOperation(packetTypeId, subValues);
    return (packetVersion, result);
}

long ComputeOperation(int type, List<long> values)
{
    switch (type)
    {
        case 0:
            return values.Sum();
        case 1:
            return values.Aggregate((a, b) => a * b);
        case 2:
            return values.Min();
        case 3:
            return values.Max();
        case 5:
            return values[0] > values[1] ? 1 : 0;
        case 6:
            return values[0] < values[1] ? 1 : 0;
        case 7:
            return values[0] == values[1] ? 1 : 0;
    }
    throw new Exception("Unknown operation type");
}

long ReadImmediateValue(string input, ref int index)
{
    bool keepGoing;
    long value = 0;
    do {
        keepGoing = ReadInt(input, ref index, 1) == 1;
        value = value << 4 | (long)ReadInt(input, ref index, 4);
    } while (keepGoing);
    return value;
}

int HexToDecimal(char c) => (c >= 'A' && c <= 'F') ? (c - 'A' + 10) : (c - '0');

int ReadInt(string input, ref int index, int length)
{
    int result = 0;
    while (length --> 0)
    {
        int charIndex = index / 4;
        int bitIndex = 3 - index % 4;
        int bit = (HexToDecimal(input[charIndex]) & (1 << bitIndex)) > 0 ? 1 : 0;
        result = result << 1 | bit;
        index += 1;
    }
    return result;
}

string input = File.ReadAllText("input").Trim();

int index = 0;
(int version, long result) = ReadPacket(input, ref index);

Console.WriteLine(version);
Console.WriteLine(result);
