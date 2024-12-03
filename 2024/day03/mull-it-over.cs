using System.Text.RegularExpressions;

string input = File.ReadAllText("input");

List<(int index, long result)> mulInstructions = Regex.Matches(input, @"mul\((\d+),(\d+)\)")
    .Select(match => (
        index: match.Index,
        left: long.Parse(match.Groups[1].Value),
        right: long.Parse(match.Groups[2].Value)
    ))
    .Select(mul => (mul.index, mul.left * mul.right))
    .ToList();

List<(int index, bool canDo)> doInstructions = Regex.Matches(input, @"do\(\)|don't\(\)")
    .Select(match => (
        index: match.Index,
        canDo: match.Value == "do()"
    ))
    .Reverse()
    .Concat([(0, true)])
    .ToList();

bool CanExecute(int index) => doInstructions.First(x => x.index < index).canDo;

Console.WriteLine(mulInstructions.Select(mul => mul.result).Sum());
Console.WriteLine(mulInstructions.Where(mul => CanExecute(mul.index)).Select(mul => mul.result).Sum());
