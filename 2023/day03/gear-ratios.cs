var lines = File.ReadAllLines("input");

System.Text.RegularExpressions.Regex numberRegex = new(@"[0-9]+");
System.Text.RegularExpressions.Regex symbolRegex = new(@"[^.0-9\n]");

var numbers = lines.SelectMany((line, y) => numberRegex.Matches(line).Select(match => (
    value: long.Parse(match.Value),
    from: match.Index,
    to: match.Index + match.Length - 1,
    y
)).ToList()).ToList();

var symbols = lines.SelectMany((line, y) => symbolRegex.Matches(line).Select(match => (
    type: match.Value,
    x: match.Index,
    y
)).ToList()).ToList();

static bool IsAdjacent((long value, int from, int to, int y) number, (string type, int x, int y) symbol) =>
    (number.y == symbol.y - 1 || number.y == symbol.y || number.y == symbol.y + 1) && // correct y
    symbol.x >= number.from - 1 && symbol.x <= number.to + 1; // correct x

long allParts = numbers
    .Where(number => symbols.Any(symbol => IsAdjacent(number, symbol)))
    .Sum(number => number.value);
Console.WriteLine(allParts);

var gearRatios = symbols
    .Where(symbol => symbol.type == "*")
    .Select(symbol => numbers.Where(number => IsAdjacent(number, symbol)).ToList())
    .Where(adjacents => adjacents.Count == 2)
    .Select(adjacents => adjacents[0].value * adjacents[1].value)
    .Sum();
Console.WriteLine(gearRatios);
