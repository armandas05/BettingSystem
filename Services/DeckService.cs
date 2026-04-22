using BettingSystem.Data.Entities;


namespace BettingSystem.Services
{
    public class DeckService
    {
        private List<Card> _deck;
        private static readonly Random _random = new Random();
        public DeckService()
        {
            ResetDeck();
        }

        public void ResetDeck()
        {
            var suits = new[] { "Hearts", "Spades", "Diamonds", "Clubs" };
            var ranks = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            _deck = new List<Card>();

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    _deck.Add(new Card{ Suit = suit, Rank = rank });

                }
            }
            _deck = _deck.OrderBy(x => _random.Next()).ToList();
        }

        public Card DrawCard()
        {
            if(_deck.Count == 0)
            {
                ResetDeck();
            }

            var card = _deck[0];
            _deck.RemoveAt(0);
            return card;

        }
    }
 }