using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BettingSystem.Data.Models
{
    public class GameHistoryDto
    {
        public int GameSessionID { get; set; }
        public int UserID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal BetAmount { get; set; }
        public decimal AmountWon { get; set; }
        public WinType Result { get; set; }
        public int GameID { get; set; }
    }
}
