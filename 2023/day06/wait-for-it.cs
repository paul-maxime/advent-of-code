var numericalRegex = new System.Text.RegularExpressions.Regex("([0-9]+)");

var input = File.ReadAllLines("input")
    .Select(line => numericalRegex.Matches(line).ToList())
    .Select(matches => matches.Select(x => long.Parse(x.Value)).ToList())
    .ToList();

var races = Enumerable.Range(0, input[0].Count)
    .Select(i => (time: input[0][i], distance: input[1][i]))
    .ToList();

static int GetRecordsForRace((long time, long distance) race) =>
    Enumerable.Range(0, (int)race.time)
        .Select(t => t * (race.time - t))
        .Where(d => d > race.distance)
        .Count();

Console.WriteLine(races.Select(GetRecordsForRace).Aggregate((a, b) => a * b));

var realRace = races.Aggregate((a, b) => (time: long.Parse(a.time + "" + b.time), distance: long.Parse(a.distance + "" + b.distance)));
Console.WriteLine(GetRecordsForRace(realRace));
