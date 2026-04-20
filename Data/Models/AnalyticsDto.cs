namespace BettingSystem.Data.Models
{
    public class AnalyticsDto
    {
        public int TotalGames { get; set; }
        public decimal TotalBetAmount { get; set; }
        public int TotalUsers { get; set; }
        public int TotalWins { get; set; }

        public List<PlayedGamesDto> PlayedGames { get; set; }
        public List<WinLoseStatsDto> WinLoseStats { get; set; }
        
        public List<GamesPerDayDto> GamesPerDayStats { get; set; }


    }
}
