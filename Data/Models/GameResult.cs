namespace BettingSystem.Data.Models
{
    public class GameResult
    {
        public decimal WonAmount { get; set; }
        public WinType Result {  get; set; }
        public decimal? Rolled { get; set; }

    }


    public enum WinType
    {
        Win,
        Lose,
        Draw
    }
}
