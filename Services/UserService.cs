using BCrypt.Net;
using BettingSystem.Data;
using BettingSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BettingSystem.Services
{
    public class UserService
    {

        private readonly AppDbContext _context;
        private readonly UserStateService _stateService;
        public UserService(AppDbContext context, UserStateService stateService)
        {
            _context = context;
            _stateService = stateService;
        }

        public async Task RegisterUser(RegisterDto dto)
        {

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
            {
                throw new Exception("User with this email already exists!");
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Balance = 0,
                DateCreated = DateTime.Now,
                IsVerified = true,
                Role = "User",
                GamesPlayed = 0,
                TotalDeposited = 0,
                GameHistories = new List<GameHistory>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }


        public async Task<User> LoginUser(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if(user == null)
            {
                throw new Exception("No account registered with this email!");
            }


            if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new Exception("Wrong password!");
            }

            _stateService.Login(user);
            return user;

        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                throw new Exception("No user found!");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }


        public async Task<PagedResult<UserDto>> GetUsers(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize)
        {
            var query = _context.Users.AsQueryable();

            //search box
            if(!string.IsNullOrWhiteSpace(searchInput))
            {
                searchInput = searchInput.Trim().ToLower();

                int userId;
                bool isIdSearch = int.TryParse(searchInput, out userId);


                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(searchInput) ||
                    u.LastName.ToLower().Contains(searchInput) ||
                    u.Email.ToLower().Contains(searchInput) ||
                    (isIdSearch && u.UserID == userId)

                );

            }

            //sort
            query = sortBy.ToLower() switch
            {
                "userid" => sortDir == "asc"
                ? query.OrderBy(u => u.UserID)
                : query.OrderByDescending(u => u.UserID),

                "firstname" => sortDir == "asc"
                ? query.OrderBy(u => u.FirstName)
                : query.OrderByDescending(u => u.FirstName),

                "lastname" => sortDir == "asc"
                ? query.OrderBy(u => u.LastName)
                : query.OrderByDescending(u => u.LastName),

                "email" => sortDir == "asc"
                ? query.OrderBy(u => u.Email)
                : query.OrderByDescending(u => u.Email),

                "balance" => sortDir == "asc"
                ? query.OrderBy(u => u.Balance)
                : query.OrderByDescending(u => u.Balance),

                "datecreated" => sortDir == "asc"
                ? query.OrderBy(u => u.DateCreated)
                : query.OrderByDescending(u => u.DateCreated),

                "isverified" => sortDir == "asc"
                ? query.OrderBy(u => u.IsVerified)
                : query.OrderByDescending(u => u.IsVerified),

                "role" => sortDir == "asc"
                ? query.OrderBy(u => u.Role)
                : query.OrderByDescending(u => u.Role),

                "gamesplayed" => sortDir == "asc"
                ? query.OrderBy(u => u.GamesPlayed)
                : query.OrderByDescending(u => u.GamesPlayed),

                "totaldeposited" => sortDir == "asc"
                ? query.OrderBy(u => u.TotalDeposited)
                : query.OrderByDescending(u => u.TotalDeposited),

            };

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Balance = u.Balance,
                    DateCreated = u.DateCreated,
                    IsVerified = u.IsVerified,
                    Role = u.Role,
                    GamesPlayed = u.GamesPlayed,
                    TotalDeposited = u.TotalDeposited,

                })
                .ToListAsync();

            return new PagedResult<UserDto>
            {
                Data = users,
                TotalCount = totalCount
            };
                
        }


        public async Task<PagedResult<GameHistoryDto>> GetGameHistories(
            string? searchInput,
            string sortBy,
            string sortDir,
            int page,
            int pageSize)
        {
            var query = _context.GameHistories.AsQueryable();

            //search box
            if (!string.IsNullOrWhiteSpace(searchInput))
            {
                searchInput = searchInput.Trim().ToLower();

                int userId;
                bool isIdSearch = int.TryParse(searchInput, out userId);


                query = query.Where(u => (isIdSearch && u.UserID == userId));

            }

            //sort
            query = sortBy.ToLower() switch
            {
                "gamesessionid" => sortDir == "asc"
                ? query.OrderBy(u => u.GameSessionID)
                : query.OrderByDescending(u => u.GameSessionID),

                "userid" => sortDir == "asc"
                ? query.OrderBy(u => u.UserID)
                : query.OrderByDescending(u => u.UserID),

                "datetime" => sortDir == "asc"
                ? query.OrderBy(u => u.DateTime)
                : query.OrderByDescending(u => u.DateTime),

                "betamount" => sortDir == "asc"
                ? query.OrderBy(u => u.BetAmount)
                : query.OrderByDescending(u => u.BetAmount),

                "amountwon" => sortDir == "asc"
                ? query.OrderBy(u => u.AmountWon)
                : query.OrderByDescending(u => u.AmountWon),

                "result" => sortDir == "asc"
                ? query.OrderBy(u => u.Result)
                : query.OrderByDescending(u => u.Result),

                "gameid" => sortDir == "asc"
                ? query.OrderBy(u => u.GameID)
                : query.OrderByDescending(u => u.GameID)

            };

            var totalCount = await query.CountAsync();

            var histories = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new GameHistoryDto
                {
                    GameSessionID = u.GameSessionID,
                    UserID = u.UserID,
                    DateTime = u.DateTime,
                    BetAmount = u.BetAmount,
                    AmountWon = u.AmountWon,
                    Result = u.Result,
                    GameID = u.GameID,
                })
                .ToListAsync();

            return new PagedResult<GameHistoryDto>
            {
                Data = histories,
                TotalCount = totalCount
            };

        }





        public async Task<decimal> GetBalance(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user.Balance;
        }

        public async Task<bool> PlaceBet(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);

            if (amount <= 0)
            {
                return false;
            }

            if (user.Balance < amount)
            {
                return false;
            }

            user.Balance -= amount;

            await _context.SaveChangesAsync();

            return true;


        }

        public async Task AddBalance(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);

            user.Balance += amount;

            await _context.SaveChangesAsync();
        }



    }
}
