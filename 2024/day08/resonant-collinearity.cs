string[] map = File.ReadAllLines("input");
int mapHeight = map.Length;
int mapWidth = map[0].Length;

Dictionary<char, List<(int x, int y)>> antennas = [];

void ParseAntennas()
{
    for (int x = 0; x < mapWidth; x++)
    {
        for (int y = 0; y < mapHeight; y++)
        {
            char c = map[y][x];
            if (c != '.')
            {
                if (!antennas.ContainsKey(c)) antennas[c] = [];
                antennas[c].Add((x, y));
            }
        }
    }
}

bool IsOnMap((int x, int y) point) => point.x >= 0 && point.y >= 0 && point.x < mapWidth && point.y < mapHeight;

long CountAntinodes(bool allowResonantHarmonics)
{
    HashSet<(int x, int y)> antinodes = [];

    foreach (char c in antennas.Keys)
    {
        foreach (var antenna1 in antennas[c])
        {
            foreach (var antenna2 in antennas[c])
            {
                if (antenna1 == antenna2) continue;

                (int x, int y) delta = (antenna1.x - antenna2.x, antenna1.y - antenna2.y);

                // Only process antenna1 (1 -> 2) because antenna2 (2 -> 1) will automatically be processed later.

                if (!allowResonantHarmonics)
                {
                    (int x, int y) antinode = (antenna1.x + delta.x, antenna1.y + delta.y);
                    if (IsOnMap(antinode)) antinodes.Add(antinode);
                }
                else
                {
                    (int x, int y) antinode = antenna1;
                    do
                    {
                        antinodes.Add(antinode);
                        antinode = (antinode.x + delta.x, antinode.y + delta.y);
                    } while (IsOnMap(antinode));
                }
            }
        }
    }

    return antinodes.Count;
}

ParseAntennas();
Console.WriteLine(CountAntinodes(false));
Console.WriteLine(CountAntinodes(true));
