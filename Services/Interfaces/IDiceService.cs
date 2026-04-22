using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IDiceService
    {
        GameResult Play(
            decimal betAmount,
            decimal rollNumber,
            string rollType);
        decimal GetMultiplier(
            decimal rollNumber,
            string rollType);
    }
}
