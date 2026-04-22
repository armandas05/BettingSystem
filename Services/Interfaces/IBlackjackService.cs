using BettingSystem.Data.Entities;

namespace BettingSystem.Services.Interfaces
{
    public interface IBlackjackService
    {
        void StartGame();
        void RestartGame();
        List<Card> ShowDealerCards();
        List<Card> ShowPlayerCards();
        int GetDealerScore();
        int GetPlayerScore();
        void Hit();
        void Stand();
        void SetBet(decimal amount);
        decimal GetBet();
        string CheckGameStatus();
    }
}
