string winds = File.ReadAllText("input").Trim();

string[][] SHAPES = {
    new[] { "####" },
    new[] { ".#.", "###", ".#." },
    new[] { "..#", "..#", "###" },
    new[] { "#", "#", "#", "#" },
    new[] { "##", "##" },
};

int currentWind = 0;
int currentShape = 0;

var rocks = new HashSet<(int x, int y)>();
int towerHeight = 0;

bool CanPlace(string[] shape, HashSet<(int x, int y)> rocks, int x, int y)
{
    for (int sy = 0; sy < shape.Length; sy++)
    {
        for (int sx = 0; sx < shape[sy].Length; sx++)
        {
            if (shape[sy][sx] == '.') continue;
            if (x + sx < 1 || x + sx > 7 || y - sy < 1) return false;
            if (rocks.Contains((x + sx, y - sy))) return false;
        }
    }
    return true;
}

int repeatsEvery = 1725;
int[] repeats = new int[repeatsEvery];
int repeatIndex = 0;

long currentStep;
for (currentStep = 0; currentStep < 10000 || repeatIndex != 0; currentStep++)
{
    var shape = SHAPES[currentShape];
    currentShape = (currentShape + 1) % SHAPES.Length;

    int x = 3;
    int y = towerHeight + 3 + shape.Length;

    int previous = towerHeight;
    while (true)
    {
        int direction = winds[currentWind] == '<' ? -1 : 1;
        currentWind = (currentWind + 1) % winds.Length;

        if (CanPlace(shape, rocks, x + direction, y))
        {
            x += direction;
        }

        if (CanPlace(shape, rocks, x, y - 1))
        {
            y -= 1;
        }
        else
        {
            for (int sy = 0; sy < shape.Length; sy++)
            {
                for (int sx = 0; sx < shape[sy].Length; sx++)
                {
                    if (shape[sy][sx] == '.') continue;
                    towerHeight = Math.Max(towerHeight, y - sy);
                    rocks.Add((x + sx, y - sy));
                }
            }
            break;
        }
    }

    if (currentStep == 2022)
    {
        Console.WriteLine($"{currentStep} -> {towerHeight}");
    }

    int delta = towerHeight - previous;

    repeats[repeatIndex] = delta;
    repeatIndex = (repeatIndex + 1) % repeats.Length;
}

long megaHeight = towerHeight;

long missing = 1000000000000 - currentStep;

long jumps = missing / repeats.Length;

currentStep += jumps * repeats.Length;
megaHeight += jumps * repeats.Sum();
missing = 1000000000000 - currentStep;

for (long i = 0; i < missing; i++)
{
    megaHeight += repeats[i];
    currentStep++;
}

Console.WriteLine($"{currentStep} -> {megaHeight}");
