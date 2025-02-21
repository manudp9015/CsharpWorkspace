using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication.Model
{

    internal class User
    {
        public string name { get; set; }
        public int age { get; set; }
        public string phoneNumber { get; set; }
        public string userEmail { get; set; }
        public string password { get; set; }
        public decimal walletBalance { get; set; }
    }

    internal class UserLumpsumInvest
    {
        public string fundName { get; set; }
        public decimal quantity { get; set; }
        public decimal investedAmount { get; set; }
        public int durationInMonths { get; set; }
        public DateTime lumpsumStartDate { get; set; }
        public DateTime lumpsumEndDate { get; set; }
        public decimal currentAmount { get; set; }

    }

    internal class UserSipInvest
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
