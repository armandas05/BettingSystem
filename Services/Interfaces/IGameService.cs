using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IGameService
    {
        Task<IResult> PlayDiceAsync(
            GameDto dto,
            int userId);
        Task<IResult> FinishBlackjackAsync(
            GameResult result,
            int userId);
    }
}
