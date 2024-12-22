Dictionary<char, (int x, int y)> numericKeypad = new() {
    ['7'] = (0, 0), ['8'] = (1, 0), ['9'] = (2, 0),
    ['4'] = (0, 1), ['5'] = (1, 1), ['6'] = (2, 1),
    ['1'] = (0, 2), ['2'] = (1, 2), ['3'] = (2, 2),
    ['0'] = (1, 3), ['A'] = (2, 3),
};

Dictionary<(int x, int y), char> numericKeypadReverse = new() {
    [(0, 0)] = '7', [(1, 0)] = '8', [(2, 0)] = '9',
    [(0, 1)] = '4', [(1, 1)] = '5', [(2, 1)] = '6',
    [(0, 2)] = '1', [(1, 2)] = '2', [(2, 2)] = '3',
    [(1, 3)] = '0', [(2, 3)] = 'A',
};

Dictionary<char, (int x, int y)> directionalKeypad = new() {
    ['^'] = (1, 0), ['A'] = (2, 0),
    ['<'] = (0, 1), ['v'] = (1, 1), ['>'] = (2, 1),
};

Dictionary<(int x, int y), char> directionalKeypadReverse = new() {
    [(1, 0)] = '^', [(2, 0)] = 'A',
    [(0, 1)] = '<', [(1, 1)] = 'v', [(2, 1)] = '>',
};

string[] codes = File.ReadAllLines("input");
Console.WriteLine(codes.Select(code => ComputeComplexity(code, 2)).Sum());
Console.WriteLine(codes.Select(code => ComputeComplexityP2(code, 25)).Sum());

// P1

(string, char)? LineFromTo(Dictionary<(int x, int y), char> keypadReverse, (int x, int y) from, (int x, int y) to, char movementChar)
{
    string movement = "";
    char next = '?';
    while (from != to)
    {
        from = (from.x + Math.Sign(to.x - from.x), from.y + Math.Sign(to.y - from.y));
        if (!keypadReverse.TryGetValue(from, out next))
        {
            return null;
        }
        movement += movementChar;
    }
    return (movement, next);

}

List<string> MovementFromTo(Dictionary<(int x, int y), char> keypadReverse, (int x, int y) from, (int x, int y) to)
{
    if (from == to) return ["A"];

    List<string> movements = [];
    if (from.x != to.x && from.y != to.y)
    {
        (string movement, char next)? line1 = LineFromTo(keypadReverse, from, (to.x, from.y), from.x < to.x ? '>' : '<');
        if (line1.HasValue)
        {
            (string movement, char next)? line2 = LineFromTo(keypadReverse, (to.x, from.y), to, from.y < to.y ? 'v' : '^');
            if (line2.HasValue) movements.Add(line1.Value.movement + line2.Value.movement + "A");
        }

        line1 = LineFromTo(keypadReverse, from, (from.x, to.y), from.y < to.y ? 'v' : '^');
        if (line1.HasValue)
        {
            (string movement, char next)? line2 = LineFromTo(keypadReverse, (from.x, to.y), to, from.x < to.x ? '>' : '<');
            if (line2.HasValue) movements.Add(line1.Value.movement + line2.Value.movement + "A");
        }
    }
    else if (from.x != to.x)
    {
        (string movement, char next)? line = LineFromTo(keypadReverse, from, to, from.x < to.x ? '>' : '<');
        if (line.HasValue) movements.Add(line.Value.movement + "A");
    }
    else if (from.y != to.y)
    {
        (string movement, char next)? line = LineFromTo(keypadReverse, from, to, from.y < to.y ? 'v' : '^');
        if (line.HasValue) movements.Add(line.Value.movement + "A");
    }
    return movements;
}

List<string> ComputeSequences(bool isDirectional, ReadOnlySpan<char> code, char current = 'A')
{
    if (code.IsEmpty)
    {
        return [""];
    }

    var keypad = isDirectional ? directionalKeypad : numericKeypad;
    var keypadReverse = isDirectional ? directionalKeypadReverse : numericKeypadReverse;

    (int x, int y) currentPos = keypad[current];
    (int x, int y) targetPos = keypad[code[0]];

    List<string> movements = MovementFromTo(keypadReverse, currentPos, targetPos);

    List<string> sequences = [];
    foreach (string movement in movements)
    {
        sequences.AddRange(ComputeSequences(isDirectional, code[1..], code[0]).Select(x => movement + x));
    }
    return sequences;
}

int ComputeBestSequence(string code, int depth)
{
    List<string> sequences = ComputeSequences(false, code);

    for (int i = 0; i < depth; i++)
    {
        sequences = sequences.SelectMany(seq => ComputeSequences(true, seq)).ToList();
    }

    int minLength = sequences.Select(x => x.Length).Min();
    return minLength;
}

long ComputeComplexity(string code, int depth)
{
    long minLength = ComputeBestSequence(code, 2);
    long codeValue = long.Parse(code.Replace("A", ""));
    return minLength * codeValue;
}

// P2

Node ComputeNodesFromString(bool isDirectional, string code)
{
    var keypad = isDirectional ? directionalKeypad : numericKeypad;
    var keypadReverse = isDirectional ? directionalKeypadReverse : numericKeypadReverse;

    char current = 'A';

    List<Node> children = [];
    foreach (char target in code)
    {
        List<string> movements = MovementFromTo(keypadReverse, keypad[current], keypad[target]);

        Node node;
        if (movements.Count > 1)
        {
            node = new BranchingNode() { Children = movements.Select(x => new ItemNode { Value = x }).ToList<Node>() };
        }
        else
        {
            node = new ItemNode { Value = movements[0] };
        }

        children.Add(node);
        current = target;
    }

    if (children.Count == 1)
    {
        return children[0];
    }

    ConcatNode concatNode = new();
    concatNode.InsertList(children);
    return concatNode;
}

Node ComputeNodesFromNode(bool isDirectional, Node node)
{
    if (node is ItemNode itemNode)
    {
        return ComputeNodesFromString(isDirectional, itemNode.Value);
    }
    if (node is BranchingNode branchingNode)
    {
        BranchingNode newNode = new();

        foreach (var subNode in branchingNode.Children)
        {
            newNode.Children.Add(ComputeNodesFromNode(isDirectional, subNode));
        }

        // Only keep the smallest branches, we can discard the others.
        long minLength = newNode.Children.Select(x => x.MinLength()).Min();
        newNode.Children = newNode.Children.Where(x => x.MinLength() == minLength).ToList();

        if (newNode.Children.Count == 1)
        {
            return newNode.Children[0];
        }

        return newNode;
    }
    if (node is ConcatNode concatNode)
    {
        ConcatNode newConcatNode = new();

        foreach (var subNode in concatNode.Children)
        {
            Node computedSubNode = ComputeNodesFromNode(isDirectional, subNode.Key);
            if (computedSubNode is ConcatNode computedConcatNode)
            {
                newConcatNode.InsertDictionary(computedConcatNode.Children, subNode.Value);
            }
            else
            {
                newConcatNode.Insert(computedSubNode, subNode.Value);
            }
        }

        return newConcatNode;
    }

    throw new Exception("Unknown node type");
}

long ComputeBestSequenceP2(string code, int depth)
{
    Node node = ComputeNodesFromString(false, code);

    for (int i = 0; i < depth; i++)
    {
        node = ComputeNodesFromNode(true, node);
    }

    return node.MinLength();
}

long ComputeComplexityP2(string code, int depth)
{
    long minLength = ComputeBestSequenceP2(code, depth);
    long codeValue = long.Parse(code.Replace("A", ""));
    return minLength * codeValue;
}

abstract class Node
{
    public abstract long MinLength();
}

class ItemNode : Node
{
    public string Value = "";

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ItemNode itemNode)
        {
            return itemNode.Value == Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override long MinLength()
    {
        return Value.Length;
    }
}

class ConcatNode : Node
{
    public Dictionary<Node, long> Children = [];

    public void Insert(Node node, long multiplier)
    {
        Children[node] = Children.GetValueOrDefault(node, 0) + multiplier;
    }

    public void InsertList(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            Children[node] = Children.GetValueOrDefault(node, 0) + 1;
        }
    }

    public void InsertDictionary(Dictionary<Node, long> nodes, long multiplier)
    {
        foreach (var node in nodes)
        {
            Children[node.Key] = Children.GetValueOrDefault(node.Key, 0) + node.Value * multiplier;
        }
    }

    public override string ToString()
    {
        return "[" + string.Join("+", Children.Select(x => $"{x.Key}*{x.Value}")) + "]";
    }

    public override bool Equals(object? obj)
    {
        if (obj is ConcatNode concatNode)
        {
            if (concatNode.Children.Count != Children.Count) return false;
            foreach (var child in Children)
            {
                if (!concatNode.Children.TryGetValue(child.Key, out long count))
                {
                    return false;
                }
                if (child.Value != count) return false;
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        // Sorting by hashcode is probably a bad idea, but it works well enough here.
        foreach (var key in Children.Keys.OrderBy(x => x.GetHashCode()))
        {
            hashCode.Add((key, Children[key]));
        }
        return hashCode.ToHashCode();
    }

    public override long MinLength()
    {
        return Children.Select(child => child.Key.MinLength() * child.Value).Sum();
    }
}

class BranchingNode : Node
{
    public List<Node> Children = [];

    public override string ToString()
    {
        return "(" + string.Join("|", Children.Select(x => x.ToString())) + ")";
    }

    public override bool Equals(object? obj)
    {
        if (obj is BranchingNode branchingNode)
        {
            if (branchingNode.Children.Count != Children.Count) return false;
            for (int i = 0; i < Children.Count; i++)
            {
                if (!Children[i].Equals(branchingNode.Children[i])) return false;
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (Node children in Children)
        {
            hashCode.Add(children);
        }
        return hashCode.ToHashCode();
    }

    public override long MinLength()
    {
        return Children.Select(x => x.MinLength()).Min();
    }
}
