using BettingSystem.Data.Models;
using BettingSystem.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

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
