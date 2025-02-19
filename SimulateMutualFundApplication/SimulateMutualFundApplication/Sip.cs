using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class Sip
    {
        private MutualFundBusiness _fundBussines;
        private UserSipInvest _userSip;
        public Sip(MutualFundBusiness fundBussines,UserSipInvest userSip)
        {
            _fundBussines = fundBussines;
            _userSip = userSip;
        }
        public void SIPInvest(string fundName)
        {
            try
            {
                _userSip.fundName=fundName;
                int amountAttempts = 0;
                Console.WriteLine("\n--- SIP Investment ---");

                decimal sipAmount;
                Console.Write("Enter SIP Amount (Min ₹500): ");
                while (!decimal.TryParse(Console.ReadLine(), out sipAmount) || sipAmount < 500)
                {
                    amountAttempts++;
                    if (amountAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for amount."); return;
                    }
                    Console.Write("Invalid amount. Enter a valid SIP amount (Min ₹500): ");
                }
                _userSip.sipAmount = sipAmount;
                int dateAttempts = 0;
                DateTime sipStartDate;
                Console.Write("Enter SIP Start Date (yyyy-MM-dd): ");
                while (!DateTime.TryParse(Console.ReadLine(), out sipStartDate) || sipStartDate < DateTime.Today)
                {
                    dateAttempts++;
                    if (dateAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for date."); return;
                    }
                    Console.Write("Invalid date. Enter a valid date (yyyy-MM-dd): ");
                }
                _userSip.sipStartDate = sipStartDate;
                int durationAttempts = 0;
                int durationInMonths;
                Console.Write("Enter SIP Duration In Months (Min 12 Months): ");
                while (!int.TryParse(Console.ReadLine(), out durationInMonths) || durationInMonths < 12)
                {
                    durationAttempts++;
                    if (durationAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for duration."); return;
                    }
                    Console.Write("Invalid duration. Enter a valid number of years (Min 1 Year): ");
                }
                _userSip.durationInMonths = durationInMonths;
                _userSip.sipEndDate = _userSip.sipStartDate.AddYears(_userSip.durationInMonths);
                _userSip.nextInstallmentDate = _userSip.sipStartDate.AddMonths(1);

                _fundBussines.SaveSIPInvest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
