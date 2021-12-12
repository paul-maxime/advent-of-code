List<(string from, string to)> paths = File.ReadAllLines("input")
    .Select(line => line.Split("-"))
    .SelectMany(x => new[] { (x[0], x[1]), (x[1], x[0]) })
    .ToList();

bool IsLargeCave(string cave) => char.IsUpper(cave[0]);
bool IsStartOrEnd(string cave) => cave == "start" || cave == "end";
bool CanVisit(List<string> visited, string cave) => IsLargeCave(cave) || !visited.Contains(cave);

int TravelThroughCave(List<(string from, string to)> paths, List<string> visitedCaves, string currentCave, bool canVisitSmallTwice)
{
    visitedCaves = visitedCaves.ToList();
    visitedCaves.Add(currentCave);

    List<string> nextCaves = paths
        .Where(path => path.from == currentCave)
        .Where(path => CanVisit(visitedCaves, path.to) || (!IsStartOrEnd(path.to) && canVisitSmallTwice))
        .Select(path => path.to)
        .ToList();

    if (currentCave == "end") return 1;
    if (nextCaves.Count == 0) return 0;

    return nextCaves
        .Select(nextCave => TravelThroughCave(
            paths, visitedCaves, nextCave,
            canVisitSmallTwice && CanVisit(visitedCaves, nextCave))
        ).Sum();
}

Console.WriteLine(TravelThroughCave(paths, new List<string>(), "start", false));
Console.WriteLine(TravelThroughCave(paths, new List<string>(), "start", true));
