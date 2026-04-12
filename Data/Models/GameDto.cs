namespace BettingSystem.Data.Models
{
    public class GameDto
    {
        public int GameID { get; set; }
        public decimal BetAmount { get; set; }

        public decimal? RollNumber { get; set; }

        public string? RollType { get; set; }

    }
}
