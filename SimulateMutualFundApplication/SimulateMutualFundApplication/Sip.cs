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

        public Sip(MutualFundBusiness fundBussines, UserSipInvest userSip)
        {
            _fundBussines = fundBussines;
            _userSip = userSip;
        }

        public void SIPInvest(string fundName)
        {
            try
            {
                _userSip.fundName = fundName;
                Console.WriteLine("\n--- SIP Investment ---");

                if (!GetValidSipAmount(out decimal sipAmount))
                {
                    return; 
                }
                if (!GetValidStartDate(out DateTime sipStartDate))
                {
                    return; 
                }
                if (!GetValidDuration(out int durationInMonths))
                {
                    return; 
                }
                ProcessSipInvestment(sipAmount, sipStartDate, durationInMonths);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private bool GetValidSipAmount(out decimal sipAmount)
        {
            int maxAttempts = 3;
            int attempts = 0;
            sipAmount = 0;
            bool validAmount = false;

            while (attempts < maxAttempts && !validAmount)
            {
                Console.Write($"Enter SIP Amount (Min ₹500): ");
                if (decimal.TryParse(Console.ReadLine(), out sipAmount) && sipAmount >= 500)
                {
                    validAmount = true;
                    _userSip.sipAmount = sipAmount;
                }
                else
                {
                    attempts++;
                    if (attempts < maxAttempts)
                    {
                        Console.WriteLine("Invalid amount. Minimum SIP amount is ₹500. Please try again.");
                    }
                    else
                    {
                        Console.WriteLine($"Maximum attempts ({maxAttempts}) reached for amount. Investment cancelled.");
                    }
                }
            }
            return validAmount;
        }

        private bool GetValidStartDate(out DateTime sipStartDate)
        {
            int maxAttempts = 3;
            int attempts = 0;
            sipStartDate = DateTime.MinValue;
            bool validDate = false;

            while (attempts < maxAttempts && !validDate)
            {
                Console.Write($"Enter SIP Start Date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out sipStartDate) && sipStartDate >= DateTime.Today)
                {
                    validDate = true;
                    _userSip.sipStartDate = sipStartDate;
                }
                else
                {
                    attempts++;
                    if (attempts < maxAttempts)
                    {
                        Console.WriteLine("Invalid date. Must be today or later (yyyy-MM-dd). Please try again.");
                    }
                    else
                    {
                        Console.WriteLine($"Maximum attempts ({maxAttempts}) reached for date. Investment cancelled.");
                    }
                }
            }

            return validDate;
        }

        private bool GetValidDuration(out int durationInMonths)
        {
            int maxAttempts = 3;
            int attempts = 0;
            durationInMonths = 0;
            bool validDuration = false;
            const int maxDurationMonths = 1200; // 100 years

            while (attempts < maxAttempts && !validDuration)
            {
                Console.Write($"Enter SIP Duration In Months (minimum 12, maximum {maxDurationMonths}): ");
                if (int.TryParse(Console.ReadLine(), out durationInMonths) && durationInMonths >= 12 && durationInMonths <= maxDurationMonths)
                {
                    validDuration = true;
                }
                else
                {
                    attempts++;
                    if (attempts < maxAttempts)
                    {
                        Console.WriteLine($"Invalid duration. Must be between 12 and {maxDurationMonths} months. Please try again.");
                    }
                    else
                    {
                        Console.WriteLine($"Maximum attempts ({maxAttempts}) reached for duration. Investment cancelled.");
                    }
                }
            }

            return validDuration;
        }

        private void ProcessSipInvestment(decimal sipAmount, DateTime sipStartDate, int durationInMonths)
        {
            Console.Write("Confirm SIP investment? (yes/no): ");
            string confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "yes")
            {
                _userSip.durationInMonths = durationInMonths;
                _userSip.sipEndDate = sipStartDate.AddMonths(durationInMonths);
                _userSip.nextInstallmentDate = sipStartDate.AddMonths(1);
                _fundBussines.SaveSIPInvest();
            }
            else
            {
                Console.WriteLine("Investment cancelled.");
            }
        }
    }
}