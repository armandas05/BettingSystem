using BettingSystem.Data;
using BettingSystem.Data.Models;
using BettingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BettingSystem.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IRabbitMqService _rabbitMqService;

        public TransactionService(AppDbContext context, IUserService userService, IRabbitMqService rabbitMqService)
        {
            _context = context;
            _userService = userService;
            _rabbitMqService = rabbitMqService;
        }
        public async Task DepositAsync(int userId, decimal amount, DepositMethod method)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            await _userService.AddBalanceAsync(userId, amount);

            user.TotalDeposited += amount;

            await _context.SaveChangesAsync();

            var transactionEvent = new TransactionEventDto
            {
                UserID = userId,
                DepositAmount = amount,
                Method = method,
                DateDeposited = DateTime.Now
            };

            await _rabbitMqService.PublishAsync("transaction-history", transactionEvent);
        }
        public async Task<PagedResult<TransactionDto>> GetAllTransactionsAsync(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize)
        {
            var query = _context.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInput))
            {
                searchInput = searchInput.Trim().ToLower();

                int userId;
                bool isIdSearch = int.TryParse(searchInput, out userId);

                query = query.Where(u => (isIdSearch && u.UserID == userId));
            }

            query = sortBy.ToLower() switch
            {
                "transactionid" => sortDir == "asc"
                ? query.OrderBy(u => u.TransactionID)
                : query.OrderByDescending(u => u.TransactionID),

                "userid" => sortDir == "asc"
                ? query.OrderBy(u => u.UserID)
                : query.OrderByDescending(u => u.UserID),

                "depositamount" => sortDir == "asc"
                ? query.OrderBy(u => u.DepositAmount)
                : query.OrderByDescending(u => u.DepositAmount),

                "method" => sortDir == "asc"
                ? query.OrderBy(u => u.Method)
                : query.OrderByDescending(u => u.Method),

                "datedeposited" => sortDir == "asc"
                ? query.OrderBy(u => u.DateDeposited)
                : query.OrderByDescending(u => u.DateDeposited),

            };

            var totalCount = await query.CountAsync();

            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new TransactionDto
                {
                    TransactionID = u.TransactionID,
                    UserID = u.UserID,
                    DepositAmount = u.DepositAmount,
                    Method = u.Method,
                    DateDeposited = u.DateDeposited,
                })
                .ToListAsync();

            return new PagedResult<TransactionDto>
            {
                Data = transactions,
                TotalCount = totalCount
            };
        }
    }
}
