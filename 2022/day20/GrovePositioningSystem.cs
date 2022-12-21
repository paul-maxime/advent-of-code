Node GetNode(List<Node> nodes, long index) => nodes[(int)(index % nodes.Count + nodes.Count) % nodes.Count];

List<Node> ReadNodes(long multiplier)
{
    List<Node> nodes = File.ReadAllLines("input")
        .Select(line => new Node { value = int.Parse(line) * multiplier })
        .ToList();

    for (int i = 0; i < nodes.Count; i++)
    {
        nodes[i].previous = GetNode(nodes, i - 1);
        nodes[i].next = GetNode(nodes, i + 1);
    }

    return nodes;
}

void MixNodes(List<Node> nodes, int times)
{
    var orignalOrder = nodes.ToArray();
    for (int mixing = 0; mixing < times; mixing++)
    {
        foreach (var node in orignalOrder)
        {
            Node newPrevious = GetNode(nodes, nodes.IndexOf(node) + (node.value % (nodes.Count - 1)) + (node.value < 0 ? -1 : 0));

            // don't move anything if we end up on the same node
            if (newPrevious == node) continue;

            // remove node from linked list
            node.previous.next = node.next;
            node.next.previous = node.previous;
            node.previous = null;
            node.next = null;

            // add node to linked list
            Node oldNext = newPrevious.next;
            newPrevious.next = node;
            node.previous = newPrevious;
            node.next = oldNext;
            oldNext.previous = node;

            // Restore array indexes for faster access
            Node cur = nodes[0];
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i] = cur;
                cur = cur.next;
            }
        }
    }
}

long ComputeGroveCoordinates(List<Node> nodes)
{
    int zeroIndex = nodes.FindIndex(node => node.value == 0);
    int[] indexes = { 1000, 2000, 3000 };
    return indexes.Select(delta => GetNode(nodes, zeroIndex + delta).value).Sum();
}

var nodes = ReadNodes(1);
MixNodes(nodes, 1);
Console.WriteLine(ComputeGroveCoordinates(nodes));

nodes = ReadNodes(811589153);
MixNodes(nodes, 10);
Console.WriteLine(ComputeGroveCoordinates(nodes));

class Node
{
    public long value;
    public Node previous;
    public Node next;
}
