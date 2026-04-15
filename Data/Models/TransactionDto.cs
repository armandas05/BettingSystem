namespace BettingSystem.Data.Models
{
    public class TransactionDto
    {

        public int TransactionID { get; set; }

        public int UserID { get; set; }

        public decimal DepositAmount { get; set; }

        public DepositMethod Method { get; set; }

        public DateTime DateDeposited { get; set; } = DateTime.Now;

    }
}
