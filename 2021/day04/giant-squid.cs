string[] input = File.ReadAllText("input").Split("\n\n");

int[] numbers = input[0].Trim().Split(",").Select(int.Parse).ToArray();

int[][][] bingos = input
    .Skip(1)
    .Select(bingo => bingo
        .Split("\n")
        .Select(line => line
            .Split(" ")
            .Where(x => x.Length > 0)
            .Select(int.Parse)
            .ToArray()
        ).ToArray()
    ).ToArray();

bool HasBingoWon(int[][] bingo)
{
    bool columnWon = Enumerable.Range(0, 5).Any(x => Enumerable.Range(0, 5).All(y => bingo[y][x] < 0));
    bool rowWon = Enumerable.Range(0, 5).Any(y => Enumerable.Range(0, 5).All(x => bingo[y][x] < 0));
    return columnWon || rowWon;
}

int GetBingoScore(int[][] bingo, int winningNumber)
{
    return bingo.SelectMany(x => x).Where(x => x > 0).Sum() * winningNumber;
}

void PlayNumberOnBingo(int[][] bingo, int number)
{
    foreach (int[] line in bingo)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == number)
            {
                line[i] = -number;
                return;
            }
        }
    }
}

(int, int) GetFirstAndLastWinnerScores(int[][][] bingos, int[] numbers)
{
    int firstWinner = 0;
    int lastWinner = 0;

    List<int[][]> remainingBingos = bingos.ToList();
    foreach (int number in numbers)
    {
        foreach (int[][] bingo in remainingBingos.ToList())
        {
            PlayNumberOnBingo(bingo, number);
            if (HasBingoWon(bingo))
            {
                if (firstWinner == 0)
                {
                    firstWinner = GetBingoScore(bingo, number);
                }
                else
                {
                    lastWinner = GetBingoScore(bingo, number);
                }
                remainingBingos.Remove(bingo);
            }
        }
    }

    return (firstWinner, lastWinner);
}

Console.WriteLine(GetFirstAndLastWinnerScores(bingos, numbers));
