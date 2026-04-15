namespace BettingSystem.Data.Models
{
    public class DepositDto
    {
        public decimal DepositAmount { get; set; }
        public DepositMethod Method { get; set; }
    }
}
