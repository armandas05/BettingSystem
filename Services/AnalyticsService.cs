using BettingSystem.Data;
using BettingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingSystem.Services
{
    public class AnalyticsService
    {
        private readonly AppDbContext _context;

        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsDto> GetAnalytics(int days)
        {
            var query = _context.GameHistories.AsQueryable();

            if(days > 0)
            {
                var fromDate = DateTime.Now.AddDays(-days);
                query = query.Where(x => x.DateTime >= fromDate);
            }

            var games = await query.ToListAsync();


            var totalGames = games.Count;

            var totalBet = games.Sum(x => x.BetAmount);

            var totalUsers = await _context.Users.CountAsync();

            var totalWins = games.Count(x => x.Result == WinType.Win);


            var mostGamesPlayed = games
                .GroupBy(x => x.GameID)
                .Select(g => new PlayedGamesDto
                {
                    GameName = g.Key == 1 ? "Blackjack" : "Dice",
                    Count = g.Count()
                })
                .ToList();

            var winLoseStats = games
                .GroupBy(x => x.Result)
                .Select(g => new WinLoseStatsDto
                {
                    Result = (WinType)g.Key,
                    Count = g.Count()
                })
                .ToList();

            

            var gamesPerDay = games
                .GroupBy(x => x.DateTime.DayOfWeek)
                .Select(g => new GamesPerDayDto
                {
                    WeekDay = g.Key,
                    Count = g.Count()
                })
                .ToList();


            return new AnalyticsDto
            {
                TotalGames = totalGames,
                TotalBetAmount = totalBet,
                TotalUsers = totalUsers,
                TotalWins = totalWins,
                PlayedGames = mostGamesPlayed,
                WinLoseStats = winLoseStats,
                GamesPerDayStats = gamesPerDay,
            };



        }




    }
}
