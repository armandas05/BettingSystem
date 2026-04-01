using System.ComponentModel.DataAnnotations;

namespace BettingSystem.Data.Models
{
    public class GameInformation
    {
        [Key]
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }

        public List<GameHistory> GameHistories { get; set; }

    }
}
