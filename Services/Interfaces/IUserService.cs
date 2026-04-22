using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterDto dto);
        Task<User> LoginUserAsync(LoginDto dto);
        Task DeleteUserAsync(int id);
        Task<PagedResult<UserDto>> GetUsersAsync(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize);
        Task<PagedResult<GameHistoryDto>> GetGameHistoriesAsync(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize);
        Task<decimal> GetBalanceAsync(int userId);
        Task<bool> PlaceBetAsync(int userId, decimal amount);
        Task AddBalanceAsync(int userId, decimal amount);
    }
}
