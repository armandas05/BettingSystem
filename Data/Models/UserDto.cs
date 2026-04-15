using System.ComponentModel.DataAnnotations;

namespace BettingSystem.Data.Models
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsVerified { get; set; }
        public User.UserRoles Role { get; set; }
        public int GamesPlayed { get; set; }
        public decimal TotalDeposited { get; set; }


    }
}
