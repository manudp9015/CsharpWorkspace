namespace MutualFundSimulatorService.Model.DTO
{
    public class AuthRequest
    {
        public string userEmail { get; set; }
        public string password { get; set; }
    }
    public class ExpenseRequest
    {
        public string FundName { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
    public class WalletRequest
    {
        public decimal Amount { get; set; }
    }
}