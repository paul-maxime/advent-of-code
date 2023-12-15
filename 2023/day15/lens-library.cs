string[] steps = File.ReadAllText("input").Trim().Split(",");

static long Hash(string value) =>
    value.Aggregate(0L, (value, c) => (value + c) * 17 % 256);

Console.WriteLine(steps.Select(Hash).Sum());

var boxes = new List<(string label, long focal)>[256];
for (int i = 0; i < 256; i++) boxes[i] = [];

foreach (var step in steps)
{
    if (step.Contains('='))
    {
        string[] data = step.Split('=');
        string label = data[0];
        long focal = long.Parse(data[1]);
        long hash = Hash(label);

        int index = boxes[hash].FindIndex(x => x.label == label);
        if (index >= 0)
        {
            boxes[hash].Insert(index, (label, focal));
            boxes[hash].RemoveAt(index + 1);
        }
        else
        {
            boxes[hash].Add((label, focal));
        }
    }
    else if (step.Contains('-'))
    {
        string label = step[..^1];
        long hash = Hash(label);

        int index = boxes[hash].FindIndex(x => x.label == label);
        if (index >= 0)
        {
            boxes[hash].RemoveAt(index);
        }
    }
}

Console.WriteLine(boxes.Select((box, i) => box.Select((lens, j) => (i + 1) * (j + 1) * lens.focal).Sum()).Sum());
