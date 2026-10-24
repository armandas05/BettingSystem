using BettingSystem.Domain;
using System.Globalization;

namespace BettingSystem.Data.Entities;

public sealed class Card : IComparable<Card>, IEquatable<Card>, IFormattable, ICloneable
{
    public string Suit { get; init; } = "Hearts";
    public string Rank { get; init; } = "A";

    private int RankOrder => Rank switch
    {
        "A" => CardConstants.AceRank,
        "J" => CardConstants.JackRank,
        "Q" => CardConstants.QueenRank,
        "K" => CardConstants.KingRank,
        var rank when int.TryParse(rank, out var value) && value is >= CardConstants.MinNumericRank and <= CardConstants.MaxNumericRank => value,

        _ => throw new ArgumentException("Invalid card rank.")
    };

    public int CompareTo(Card? other)
    {
        if (other is null) return 1;
        var comparison = RankOrder.CompareTo(other.RankOrder);
        if (comparison == 0) comparison = StringComparer.Ordinal.Compare(Rank, other.Rank);

        return comparison != 0 ? comparison : StringComparer.Ordinal.Compare(Suit, other.Suit);
    }

    public bool Equals(Card? other) => other is not null && Rank == other.Rank && Suit == other.Suit;
    public override bool Equals(object? obj) => obj is Card card && Equals(card);
    public override int GetHashCode() => HashCode.Combine(Rank, Suit);
    public static bool operator ==(Card? left, Card? right) => Equals(left, right);
    public static bool operator !=(Card? left, Card? right) => !Equals(left, right);
    public object Clone() => new Card { Rank = Rank, Suit = Suit };
    public void Deconstruct(out string rank, out string suit) => (rank, suit) = (Rank, Suit);
    public override string ToString() => ToString("G", CultureInfo.InvariantCulture);
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        null or "" or "G" => $"{Rank} of {Suit}",
        "R" => Rank,
        "S" => Suit,

        _ => throw new FormatException($"Unsupported card format: {format}")
    };
}
