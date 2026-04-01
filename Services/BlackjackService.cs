using BettingSystem.Data.Entities;
using BettingSystem.Services;
using System.Runtime.CompilerServices;

namespace BettingSystem.Services
{
    public class BlackjackService
    {

        private List<Card> _dealerCards = new List<Card>();
        private List<Card> _playerCards = new List<Card>();
        private readonly DeckService _deckService = new DeckService();
        private bool _isGameFinished = false;
        private decimal _currentBet;

        public BlackjackService(DeckService deckService)
        {
            _deckService = deckService;
        }
        public void StartGame()
        {
            //gameState = true;
            for (int i = 0; i < 1; i++)
            {
                _dealerCards.Add(_deckService.DrawCard());
            }

            for (int i = 0; i < 2; i++)
            {
                _playerCards.Add(_deckService.DrawCard());
            }
        }

        public void RestartGame()
        {
            _dealerCards.Clear();
            _playerCards.Clear();
            _deckService.ResetDeck();

            _isGameFinished = false;

        }

        public List<Card> ShowDealerCards() => _dealerCards;
        public List<Card> ShowPlayerCards() => _playerCards;


        public int GetDealerScore() => CalculateHandValue(_dealerCards);
        public int GetPlayerScore() => CalculateHandValue(_playerCards);


        public void Hit()
        {
            _playerCards.Add(_deckService.DrawCard());
            if (GetDealerScore() > 21) return;
            if (GetPlayerScore() > 21) return;
            if (GetPlayerScore() == 21) return;
            _dealerCards.Add(_deckService.DrawCard());
        }

        public void Stand() {
            while(GetDealerScore() < 17)
            {
                _dealerCards.Add(_deckService.DrawCard());
            }

            _isGameFinished = true;
        
        }

        public void SetBet(decimal amount)
        {
            _currentBet = amount;
        }

        public decimal GetBet() => _currentBet;


        public string CheckGameStatus()
        {

            int dealer = GetDealerScore();
            int player = GetPlayerScore();

            // bust
            if (player > 21)
                return "Dealer wins!";

            if (dealer > 21)
                return "Player wins!";

            // jei dar ne stand
            if (!_isGameFinished)
                return "Ongoing";

            // po stand – tada lyginam
            if (player > dealer)
                return "Player wins!";
            else if (dealer > player)
                return "Dealer wins!";
            else
                return "Draw!";
        }

        public int CalculateHandValue(List<Card> cards) {

            List<Card> _cards = new List<Card>();
            int cardSum = 0;
            int aceCount = 0;

            foreach(var card in cards)
            {
                if(card.Rank == "A")
                {
                    cardSum += 11;
                } 
                else if (card.Rank == "K" || card.Rank == "Q" || card.Rank == "J")
                {
                    cardSum += 10;
                } 
                else
                {
                    cardSum += int.Parse(card.Rank);
                }
            }

            while(cardSum > 21 && aceCount > 0)
            {
                cardSum -= 10;
                aceCount--;
            }

            return cardSum;
        }








    }
}
