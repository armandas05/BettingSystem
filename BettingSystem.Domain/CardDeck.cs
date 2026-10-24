using System.Collections;
using BettingSystem.Data.Entities;

namespace BettingSystem.Domain;

public sealed class CardDeck : IEnumerable<Card>
{
    private readonly Card[] _cards;
    public int Count => _cards.Length;

    public CardDeck(IEnumerable<Card> cards) => _cards = cards.Select(c => (Card)c.Clone()).ToArray();

    public Card[] Slice(Range range) => _cards[range];

    public IEnumerator<Card> GetEnumerator() => new CardEnumerator(_cards);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class CardEnumerator : IEnumerator<Card>
    {
        private readonly Card[] _cards;
        private int _index = -1;
        public CardEnumerator(Card[] cards) => _cards = cards;
        public Card Current => _index >= 0 && _index < _cards.Length
            ? _cards[_index] : throw new InvalidOperationException("Enumerator is not on a card.");
        object IEnumerator.Current => Current;
        public bool MoveNext()
        {
            if (_index < _cards.Length) _index++;

            return _index < _cards.Length;
        }
        public void Reset() => _index = -1;
        public void Dispose() { }
    }
}
