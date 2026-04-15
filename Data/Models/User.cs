using System.ComponentModel.DataAnnotations;

namespace BettingSystem.Data.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public decimal Balance { get; set; } = 0;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsVerified { get; set; } = false;
        public UserRoles Role { get; set; }
        public int GamesPlayed { get; set; } = 0;
        public decimal TotalDeposited { get; set; } = 0;


        public enum UserRoles
        {
            User,
            Admin
        }


        public List<GameHistory>? GameHistories { get; set; }
        public List<Transaction>? Transactions { get; set; }

    }
}
