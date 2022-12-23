string[] input = File.ReadAllText("input").Split("\n\n");
string[] map = input[0].Split("\n");

int mapHeight = map.Length;
int mapWidth = map.Select(x => x.Length).Max();

var path = new System.Text.RegularExpressions.Regex("([0-9]+)([RL])?")
    .Matches(input[1])
    .Select(x => (
        count: int.Parse(x.Groups[1].Value),
        rotation: x.Groups[2].Value
    )).ToList();

FlatMap.Process(map, mapWidth, mapHeight, path);
CubicMap.Process(map, mapWidth, mapHeight, path);
