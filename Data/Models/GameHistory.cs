using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BettingSystem.Data.Models
{
    public class GameHistory
    {
        [Key]
        public int GameSessionID {  get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public DateTime DateTime {get; set; } = DateTime.Now;
        public decimal BetAmount { get; set; }
        public decimal AmountWon { get; set; }
        public GameResult Result { get; set; }
        [ForeignKey("GameInformation")]
        public int GameID { get; set; }

        public User User { get; set; }
        public GameInformation GameInformation { get; set; }
    }

    public enum GameResult
    {
        Win,
        Lose
        //Draw
    }
}
