namespace BettingSystem.Data.Models
{
    public class TransactionEventDto
    {
        public int UserID { get; set; }

        public decimal DepositAmount { get; set; }

        public DepositMethod Method { get; set; }

        public DateTime DateDeposited { get; set; } = DateTime.Now;

    }
}
