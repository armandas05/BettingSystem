using BettingSystem.Data.Entities;
using BettingSystem.Domain;

namespace BettingSystem.Services;

public sealed class CardDrawnEventArgs : EventArgs
{
    public Card Card { get; }
    public CardDrawnEventArgs(Card card) => Card = card;
}

public sealed class DeckService
{
    private static readonly Card[] StandardCards;
    private readonly ILogger<DeckService> _logger;
    private CardDeck _deck = new(Array.Empty<Card>());
    public event EventHandler<CardDrawnEventArgs>? CardDrawn;

    static DeckService()
    {
        StandardCards = CardExtensions.CreateCards("Hearts", "Spades", "Diamonds", "Clubs").ToArray();
        Array.Sort(StandardCards);
        if (StandardCards.Distinct().Count() != CardConstants.StandardDeckSize || StandardCards[0] == StandardCards[1])
            throw new InvalidOperationException($"The standard deck must have {CardConstants.StandardDeckSize} unique cards.");
    }

    public DeckService(ILogger<DeckService> logger)
    {
        _logger = logger;
        CardDrawn += LogCardDrawn;
        ResetDeck();
    }

    private void LogCardDrawn(object? sender, CardDrawnEventArgs args) =>
        _logger.LogDebug("Drew {Card}; face card: {FaceCard}", args.Card.ToString("G", null), args.Card.IsFaceCard());

    public void ResetDeck()
    {
        var cards = (Card[])StandardCards.Clone();
        Random.Shared.Shuffle(cards);
        _deck = new CardDeck(cards);
    }

    public Card DrawCard()
    {
        if (_deck.Count == 0) ResetDeck();

        var card = _deck.First();

        _deck = new CardDeck(_deck.Slice(1..));
        CardDrawn?.Invoke(this, new CardDrawnEventArgs(card));
        return card;
    }
}