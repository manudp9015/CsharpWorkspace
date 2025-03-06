namespace MutualFundSimulatorService.Model.DTO
{
    public class UserDto
    {
        public string name { get; set; }
        public int age { get; set; }
        public string phoneNumber { get; set; }
        public string userEmail { get; set; }
        public string password { get; set; }
        public decimal walletBalance { get; set; }
    }
    public class SaveSIPInvestDto
    {
        public string fundName { get; set; }
        public decimal sipAmount {  get; set; } 
        public DateTime sipStartDate { get; set; }
        public int durationInMonths {  get; set; }

    }
 
}