using System;

namespace MutualFundSimulatorService.Model
{
    public class User
    {
        public string name { get; set; }
        public int age { get; set; }
        public string phoneNumber { get; set; }
        public string userEmail { get; set; }
        public string password { get; set; }
        public decimal walletBalance { get; set; }
        public static DateTime CurrentDate { get; set; }
    }

    public class UserLumpsumInvest
    {
        public string fundName { get; set; }
        public decimal quantity { get; set; }
        public decimal investedAmount { get; set; }
        public DateTime lumpsumStartDate { get; set; }
        public decimal currentAmount { get; set; }
    }

    public class UserSipInvest
    {
        public string fundName { get; set; }
        public decimal sipAmount { get; set; }
        public DateTime sipStartDate { get; set; }
        public DateTime nextInstallmentDate { get; set; }
        public DateTime sipEndDate { get; set; }
        public int durationInMonths { get; set; }
        public decimal totalUnits { get; set; }
        public int totalInstallments { get; set; }
        public decimal totalInvestedAmount { get; set; }
        public decimal currentAmount { get; set; }
    }
}