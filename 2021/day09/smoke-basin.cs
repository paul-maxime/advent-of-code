int[][] map = File.ReadAllLines("input")
  .Select(x => x.ToCharArray().Select(c => c - '0').ToArray())
  .ToArray();

bool IsLowPoint(int[][] map, int x, int y) =>
  (y == map.Length - 1 || map[y][x] < map[y + 1][x]) &&
  (y == 0 || map[y][x] < map[y - 1][x]) &&
  (x == map[0].Length - 1 || map[y][x] < map[y][x + 1]) &&
  (x == 0 || map[y][x] < map[y][x - 1]);

IEnumerable<(int x, int y)> GetLowPoints(int[][] map) =>
  Enumerable.Range(0, map.Length)
  .SelectMany(y => Enumerable.Range(0, map[0].Length).Select(x => (x, y)))
  .Where(p => IsLowPoint(map, p.x, p.y));

int GetRiskLevel(int[][] map) => GetLowPoints(map)
  .Select(p => map[p.y][p.x] + 1)
  .Sum();

int GetBasinSize(int[][] map, int x, int y, HashSet<(int, int)>? seen = null, int height = -1)
{
  if (x < 0 || x >= map[0].Length || y < 0 || y >= map.Length) return 0;
  if (map[y][x] >= 9) return 0;
  if (height >= 0 && map[y][x] <= height) return 0;

  if (seen == null) seen = new HashSet<(int, int)>();
  if (seen.Contains((x, y))) return 0;
  seen.Add((x, y));

  height = map[y][x];
  return GetBasinSize(map, x + 1, y, seen, height) +
    GetBasinSize(map, x - 1, y, seen, height) +
    GetBasinSize(map, x, y + 1, seen, height) +
    GetBasinSize(map, x , y - 1, seen, height) + 1;
}

int ComputeLargestBasins(int[][] map, int count) => GetLowPoints(map)
  .Select(p => GetBasinSize(map, p.x, p.y))
  .OrderByDescending(x => x)
  .Take(count)
  .Aggregate((a, b) => a * b);

Console.WriteLine(GetRiskLevel(map));
Console.WriteLine(ComputeLargestBasins(map, 3));
