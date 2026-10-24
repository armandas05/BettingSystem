using BettingSystem.Domain;
using BettingSystem.Data.Entities;
using BettingSystem.Services;
using BettingSystem.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace BettingSystem.Services
{
    public class BlackjackService : IBlackjackService
    {
        private List<Card> _dealerCards = new List<Card>();
        private List<Card> _playerCards = new List<Card>();
        private readonly DeckService _deckService;
        private bool _isGameFinished = false;
        private decimal _currentBet;
        private readonly HandRules _rules = new BlackjackRules();

        public BlackjackService(DeckService deckService)
        {
            _deckService = deckService;
        }
        public void StartGame()
        {
            for (int i = 0; i < BlackjackConstants.InitialDealerCards; i++)
            {
                _dealerCards.Add(_deckService.DrawCard());
            }

            for (int i = 0; i < BlackjackConstants.InitialPlayerCards; i++)
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

            if (GetPlayerScore() >= BlackjackConstants.TargetScore)
            {
                _isGameFinished = true;
            }

        }

        public void Stand() {
            while(GetDealerScore() < BlackjackConstants.DealerStandScore)
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

            if (player == BlackjackConstants.TargetScore && !_isGameFinished)
            {
                _isGameFinished = true;
                return "Player wins!";
            }

            if (player > BlackjackConstants.TargetScore)
            {
                _isGameFinished = true;
                return "Dealer wins!";
            }

            if (dealer > BlackjackConstants.TargetScore)
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

        public int CalculateHandValue(List<Card> cards) => _rules.Calculate(cards);
    }
}
