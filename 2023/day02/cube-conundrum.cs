Dictionary<string, long> CUBES = new()
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 },
};

System.Text.RegularExpressions.Regex regex = new(@"(\d+) ([a-z]+)");

var games = File.ReadAllLines("input")
    .Select((line, index) => (
        id: index + 1,
        rounds: line.Split(";").Select(round => regex.Matches(round).Select(match => (
            color: match.Groups[2].Value,
            count: int.Parse(match.Groups[1].Value)
        ))
    )));

int possibleGames = games
    .Where(game => game.rounds.All(round => round.All(cubes => cubes.count <= CUBES[cubes.color])))
    .Sum(game => game.id);
Console.WriteLine(possibleGames);

int totalPower = games.Select(game => CUBES.Keys
        .Select(color => game.rounds.SelectMany(round => round.Where(x => x.color == color).Select(x => x.count)).Max())
        .Aggregate(1, (a, b) => a * b)
    ).Sum();
Console.WriteLine(totalPower);
