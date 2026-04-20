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
            if (_isGameFinished) return;

            _playerCards.Add(_deckService.DrawCard());

            if (GetPlayerScore() >= 21)
            {
                _isGameFinished = true;
            }

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

            if (player == 21 && !_isGameFinished)
            {
                _isGameFinished = true;
                return "Player wins!";
            }

            if (player > 21)
            {
                _isGameFinished = true;
                return "Dealer wins!";
            }

            if (dealer > 21)
            {
                _isGameFinished = true;
                return "Player wins!";
            }

            if (!_isGameFinished)
                return "Ongoing";

            if (player > dealer)
                return "Player wins!";
            else if (dealer > player)
                return "Dealer wins!";
            else
                return "Draw!";
        }

        public int CalculateHandValue(List<Card> cards) {
            int cardSum = 0;
            int aceCount = 0;

            foreach(var card in cards)
            {
                if(card.Rank == "A")
                {
                    cardSum += 11;
                    aceCount++;
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
