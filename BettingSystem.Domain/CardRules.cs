using BettingSystem.Data.Entities;

namespace BettingSystem.Domain;

public abstract class HandRules
{
    public abstract int Calculate(IEnumerable<Card> cards);
}

public sealed class BlackjackRules : HandRules
{
    public override int Calculate(IEnumerable<Card> cards)
    {
        int score = 0, aces = 0;
        foreach (var card in cards)
        {
            var (rank, _) = card;
            score += rank switch
            {
                "A" => BlackjackConstants.AceHighValue,
                "K" or "Q" or "J" => BlackjackConstants.FaceCardValue,
                var number when int.TryParse(number, out var value) && value is >= CardConstants.MinNumericRank and <= CardConstants.MaxNumericRank => value,
                _ => throw new ArgumentException($"Invalid card: {card:G}")
            };
            if (rank == "A") aces++;
        }
        while (score > BlackjackConstants.TargetScore && aces-- > 0) score -= BlackjackConstants.AceAdjustment;
        return score;
    }
}

public static class CardExtensions
{
    public static bool IsFaceCard(this Card card) => card.Rank is "J" or "Q" or "K";
    public static IEnumerable<Card> CreateCards(params string[] suits)
    {
        string[] ranks = ["A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"];
        foreach (var suit in suits)
            foreach (var rank in ranks)
                yield return new Card { Suit = suit, Rank = rank };
    }
}
