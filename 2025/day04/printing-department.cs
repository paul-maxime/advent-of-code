HashSet<(int x, int y)> map = File.ReadAllLines("input")
    .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
    .Where(cell => cell.c == '@')
    .Select(cell => (cell.x, cell.y))
    .ToHashSet();

int CountAdjacentRolls(int x, int y)
{
    int count = 0;
    if (map.Contains((x - 1, y - 1))) count++;
    if (map.Contains((x - 1, y))) count++;
    if (map.Contains((x - 1, y + 1))) count++;
    if (map.Contains((x, y - 1))) count++;
    if (map.Contains((x, y + 1))) count++;
    if (map.Contains((x + 1, y - 1))) count++;
    if (map.Contains((x + 1, y))) count++;
    if (map.Contains((x + 1, y + 1))) count++;
    return count;
}

List<(int x, int y)> ReachableRolls()
{
    return map.Where(cell => CountAdjacentRolls(cell.x, cell.y) < 4).ToList();
}

bool RemoveRemoveReachableRolls()
{
    var rolls = ReachableRolls();
    map.RemoveWhere(rolls.Contains);
    return rolls.Count > 0;
}

Console.WriteLine(ReachableRolls().Count);

int rollsBefore = map.Count;
while (RemoveRemoveReachableRolls()) {}
int rollsAfter = map.Count;
Console.WriteLine(rollsBefore - rollsAfter);
