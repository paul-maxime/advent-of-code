var cards = File.ReadAllLines("input")
    .Select(line => line.Split(":")[1])
    .Select(line => line.Split("|"))
    .Select(parts => (
        winning: parts[0].Split(" ").Where(x => x.Length > 0).Select(int.Parse).ToList(),
        ours: parts[1].Split(" ").Where(x => x.Length > 0).Select(int.Parse).ToList()
    )).ToList();

int points = cards
    .Select(card => (int)Math.Floor(Math.Pow(2, card.winning.Intersect(card.ours).Count() - 1)))
    .Sum();
Console.WriteLine(points);

int GetFinalCardsCount(List<(List<int> winning, List<int> ours)> cards)
{
    int[] cardsCount = Enumerable.Repeat(1, cards.Count).ToArray();
    for (int i = 0; i < cards.Count; i++)
    {
        int matching = cards[i].winning.Intersect(cards[i].ours).Count();
        for (int j = 1; j <= matching; j++)
        {
            cardsCount[i + j] += cardsCount[i];
        }
    }
    return cardsCount.Sum();
}
Console.WriteLine(GetFinalCardsCount(cards));
