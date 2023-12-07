var cards = File.ReadAllLines("input")
    .Select(line => line.Split(" "))
    .Select(parts => (
        card: parts[0],
        bid: long.Parse(parts[1])
    ))
    .ToList();

static int GetCardType(string card, bool hasJoker)
{
    var groups = card
        .Where(c => !hasJoker || c != 'J')
        .GroupBy(x => x)
        .Select(x => x.Count())
        .OrderByDescending(x => x)
        .ToList();

    if (hasJoker)
    {
        int jokers = card.Count(c => c == 'J');
        if (jokers == 5) return 6; // five jokers
        groups[0] += jokers; // add the jokers to the largest group
    }

    if (groups.Count == 1) return 6; // five of a kind
    if (groups.Count == 2 && groups[0] == 4) return 5; // four of a kind
    if (groups.Count == 2 && groups[0] == 3 && groups[1] == 2) return 4; // full house
    if (groups.Count == 3 && groups[0] == 3) return 3; // three of a kind
    if (groups.Count == 3 && groups[0] == 2) return 2; // two pair
    if (groups.Count == 4) return 1; // one pair
    return 0; // high card
}

static int CompareCards(string cardA, string cardB, bool hasJoker)
{
    int typeA = GetCardType(cardA, hasJoker);
    int typeB = GetCardType(cardB, hasJoker);
    if (typeA != typeB) return typeA - typeB;

    string cardValues = hasJoker ? "AKQT98765432J" : "AKQJT98765432";
    for (int i = 0; i < cardA.Length; i++)
    {
        int indexA = cardValues.IndexOf(cardA[i]);
        int indexB = cardValues.IndexOf(cardB[i]);
        if (indexA != indexB) return indexB - indexA;
    }
    return 0;
}

cards.Sort((a, b) => CompareCards(a.card, b.card, false));
long totalWinnings = cards.Select((card, index) => card.bid * (index + 1)).Sum();
Console.WriteLine(totalWinnings);

cards.Sort((a, b) => CompareCards(a.card, b.card, true));
totalWinnings = cards.Select((card, index) => card.bid * (index + 1)).Sum();
Console.WriteLine(totalWinnings);
