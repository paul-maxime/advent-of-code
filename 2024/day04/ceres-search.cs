List<(int x, int y)> DIRECTIONS = [
    (-1, -1), (0, -1), (1, -1),
    (-1, 0), (1, 0),
    (-1, 1), (0, 1), (1, 1),
];

List<(int x, int y)> CROSS = [
    (-1, -1), (1, -1),
    (-1, 1), (1, 1),
];

string XMAS = "XMAS";

string[] input = File.ReadAllLines("input");

char? CellAt(int x, int y) => input.ElementAtOrDefault(y)?.ElementAtOrDefault(x);

int xmasCount = Enumerable.Range(0, input.Length)
    .SelectMany(y => Enumerable.Range(0, input[y].Length)
        .SelectMany(x => DIRECTIONS
            .Select(dir => Enumerable.Range(0, XMAS.Length)
                .Select(i => (i, x: x + dir.x * i, y: y + dir.y * i))
                .All(cell => CellAt(cell.x, cell.y) == XMAS[cell.i])
            )
        )
    ).Where(x => x).Count();

int crossMasCount = Enumerable.Range(0, input.Length)
    .SelectMany(y => Enumerable.Range(0, input[y].Length)
        .Select(x => (x, y)))
    .Where(p => CellAt(p.x, p.y) == 'A')
    .Select(p => (
        p.x,
        p.y,
        mCount: CROSS.Count(delta => CellAt(p.x + delta.x, p.y + delta.y) == 'M'),
        sCount: CROSS.Count(delta => CellAt(p.x + delta.x, p.y + delta.y) == 'S'))
    )
    .Where(p => p.mCount == 2 && p.sCount == 2)
    .Where(p => CellAt(p.x + CROSS[0].x, p.y + CROSS[0].y) != CellAt(p.x + CROSS[3].x, p.y + CROSS[3].y))
    .Count();

Console.WriteLine(xmasCount);
Console.WriteLine(crossMasCount);
