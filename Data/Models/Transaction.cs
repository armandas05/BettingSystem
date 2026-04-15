using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BettingSystem.Data.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public decimal DepositAmount { get; set; }

        [Required]
        public DepositMethod Method { get; set; }

        public DateTime DateDeposited { get; set; } = DateTime.Now;

        public User User { get; set; }
    }

    public enum DepositMethod
    {
        BTC,
        ETH,
        Bank,
        Card,
        GooglePay,
        ApplePay
    }
}