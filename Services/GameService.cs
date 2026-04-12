using BettingSystem.Data;
using BettingSystem.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BettingSystem.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;
        private readonly BlackjackService _blackjackService;
        private readonly DiceService _diceService;

        public GameService(AppDbContext context, BlackjackService blackjackService, DiceService diceService)
        {
            _context = context;
            _blackjackService = blackjackService;
            _diceService = diceService;
        }


        public async Task<IResult> PlayDice(GameDto dto, int userId)
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

            var history = new GameHistory
            {
                UserID = userId,
                BetAmount = dto.BetAmount,
                AmountWon = result.WonAmount,
                Result = result.Result,
                GameID = 2,
                DateTime = DateTime.Now
            };

            _context.GameHistories.Add(history);
            user.GamesPlayed++;

            await _context.SaveChangesAsync();
            return Results.Ok(result);

        }


        public async Task<IResult> FinishBlackjack(GameResult result, int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new Exception("User not found");

            if (result.Result == WinType.Win)
            {
                user.Balance += result.WonAmount;
            }


            var history = new GameHistory
            {
                UserID = userId,
                BetAmount = _blackjackService.GetBet(),
                AmountWon = result.WonAmount,
                Result = result.Result,
                GameID = 1,
                DateTime = DateTime.Now
            };

            _context.GameHistories.Add(history);
            user.GamesPlayed++;

            await _context.SaveChangesAsync();
            return Results.Ok(result);




        }



    }
}
