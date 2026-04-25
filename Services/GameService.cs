using BettingSystem.Data;
using BettingSystem.Data.Models;
using BettingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BettingSystem.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly IBlackjackService _blackjackService;
        private readonly IDiceService _diceService;
        private readonly IRabbitMqService _rabbitMqService;

        public GameService(AppDbContext context, IBlackjackService blackjackService, IDiceService diceService, IRabbitMqService rabbitMqService)
        {
            _context = context;
            _blackjackService = blackjackService;
            _diceService = diceService;
            _rabbitMqService = rabbitMqService;
        }
        public async Task<IResult> PlayDiceAsync(GameDto dto, int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new Exception("User not found");

            if (user.Balance < dto.BetAmount) return Results.BadRequest("Not enough balance");

            var result = _diceService.Play(dto.BetAmount, (decimal)dto.RollNumber, dto.RollType);

            user.Balance -= dto.BetAmount;

            if(result.Result == WinType.Win)
            {
                user.Balance += result.WonAmount;
            }

            user.GamesPlayed++;

            await _context.SaveChangesAsync();


            var gameEvent = new GameEventDto
            {
                UserID = userId,
                BetAmount = dto.BetAmount,
                AmountWon = result.WonAmount,
                Result = result.Result,
                GameId = 2,
                DateTime = DateTime.Now
            };

            await _rabbitMqService.PublishAsync("game-history", gameEvent);

            return Results.Ok(result);
        }

        public async Task<IResult> FinishBlackjackAsync(GameResult result, int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new Exception("User not found");

            if (result.Result == WinType.Win)
            {
                user.Balance += result.WonAmount;
            }

            user.GamesPlayed++;

            await _context.SaveChangesAsync();

            var gameEvent = new GameEventDto
            {
                UserID = userId,
                BetAmount = _blackjackService.GetBet(),
                AmountWon = result.WonAmount,
                Result = result.Result,
                GameId = 1,
                DateTime = DateTime.Now
            };

            await _rabbitMqService.PublishAsync("game-history", gameEvent);
            
            return Results.Ok(result);
        }
    }
}
