using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface ITransactionService
    {
        Task DepositAsync(
            int userId,
            decimal amount,
            DepositMethod method);
        Task<PagedResult<TransactionDto>> GetAllTransactionsAsync(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize);
    }
}
