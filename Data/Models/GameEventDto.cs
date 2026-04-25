namespace BettingSystem.Data.Models
{
    public class GameEventDto
    {
        public int UserID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal BetAmount { get; set; }
        public decimal AmountWon { get; set; }
        public WinType Result { get; set; }
        public int GameId { get; set; }
    }
}
