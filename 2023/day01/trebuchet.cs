Dictionary<string, long> DIGITS = new()
{
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
    { "4", 4 },
    { "5", 5 },
    { "6", 6 },
    { "7", 7 },
    { "8", 8 },
    { "9", 9 },
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 },
};

string[] lines = File.ReadAllLines("input");

long wrongCalibrationValue = lines.Sum(line =>
    long.Parse(line.FirstOrDefault(char.IsDigit, '0') + "" + line.LastOrDefault(char.IsDigit, '0')
));
Console.WriteLine(wrongCalibrationValue);

long correctCalibrationValue = lines.Sum(line =>
{
    var foundDigits = DIGITS.Keys.Select(digit => (
        digit,
        value: DIGITS[digit],
        first: line.IndexOf(digit),
        last: line.LastIndexOf(digit)
    )).Where(x => x.first != -1);

    var min = foundDigits.MinBy(x => x.first);
    var max = foundDigits.MaxBy(x => x.last);
    return long.Parse(min.value + "" + max.value);
});
Console.WriteLine(correctCalibrationValue);
