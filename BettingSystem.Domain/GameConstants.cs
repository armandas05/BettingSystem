namespace BettingSystem.Domain;

public static class GameIds
{
    public const int Blackjack = 1;
    public const int Dice = 2;
}

public static class CardConstants
{
    public const int StandardDeckSize = 52;
    public const int MinNumericRank = 2;
    public const int MaxNumericRank = 10;
    public const int AceRank = 1;
    public const int JackRank = 11;
    public const int QueenRank = 12;
    public const int KingRank = 13;
}

public static class BlackjackConstants
{
    public const int TargetScore = 21;
    public const int DealerStandScore = 17;
    public const int InitialDealerCards = 1;
    public const int InitialPlayerCards = 2;
    public const int AceHighValue = 11;
    public const int AceLowValue = 1;
    public const int AceAdjustment = AceHighValue - AceLowValue;
    public const int FaceCardValue = 10;
    public const decimal WinPayoutMultiplier = 2m;
}

public static class DiceConstants
{
    public const decimal MinRoll = 0m;
    public const decimal PercentageScale = 100m;
    public const decimal MaxRoll = 99.99m;
    public const decimal MinWinChance = 0.01m;
    public const decimal MaxWinChance = 95m;
    public const decimal PayoutPercentage = 95m;
    public const decimal BreakEvenMultiplier = 1m;
    public const decimal MinPayoutMultiplier = 1.01m;
    public const int DecimalPlaces = 2;
}
