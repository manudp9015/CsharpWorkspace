using MutualFundSimulatorApplication.Business;
using MutualFundSimulatorApplication.Model;
using System;

namespace MutualFundSimulatorApplication
{
    internal class Sip
    {
        private MutualFundBusiness _fundBussines;
        private UserSipInvest _userSip;
        private const decimal ExpenseRatio = 0.01m; // 1% expense ratio
        private User _user;

        public Sip(MutualFundBusiness fundBussines, UserSipInvest userSip, User user)
        {
            _fundBussines = fundBussines;
            _userSip = userSip;
            _user = user;
        }

        public void SIPInvest(string fundName)
        {
            try
            {
                _userSip.fundName = fundName;
                Console.WriteLine($"\n--- Sip Investing in {_userSip.fundName} ---");
                decimal pricePerUnit = _fundBussines.GetFundPrice(_userSip.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                if (!GetValidSipAmount(out decimal sipAmount))
                {
                    return;
                }

                // Check wallet balance
                if (_user.walletBalance < sipAmount)
                {
                    Console.WriteLine($"Insufficient funds in wallet (₹{_user.walletBalance}). Please add money to your wallet.");
                    return;
                }

                if (!GetValidDuration(out int durationInMonths))
                {
                    return;
                }

                ProcessSipInvestment(sipAmount, durationInMonths, pricePerUnit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private bool GetValidSipAmount(out decimal sipAmount)
        {
            int attempts = 0;
            sipAmount = 0;
            bool validAmount = false;

            while (attempts < 3 && !validAmount)
            {
                Console.Write($"Enter SIP amount (minimum ₹500): ");
                if (decimal.TryParse(Console.ReadLine(), out sipAmount) && sipAmount >= 500)
                {
                    validAmount = true;
                    decimal expense = sipAmount * ExpenseRatio;
                    Console.WriteLine($"SIP amount: ₹ {sipAmount}");
                    Console.WriteLine($"Expense (1%): ₹ {expense}");
                }
                else
                {
                    attempts++;
                    if (attempts < 3)
                        Console.WriteLine("Invalid amount. Minimum SIP amount is ₹500. Please try again.");
                    else
                        Console.WriteLine($"Maximum attempts reached for amount. Investment cancelled.");
                }
            }
            return validAmount;
        }

        private bool GetValidDuration(out int durationInMonths)
        {
            int attempts = 0;
            durationInMonths = 0;
            bool validDuration = false;
            const int maxDurationMonths = 1200; // 100 years

            while (attempts < 3 && !validDuration)
            {
                Console.Write($"Enter SIP duration in months (minimum 12, maximum {maxDurationMonths}): ");
                if (int.TryParse(Console.ReadLine(), out durationInMonths) && durationInMonths >= 12 && durationInMonths <= maxDurationMonths)
                    validDuration = true;
                else
                {
                    attempts++;
                    if (attempts < 3)
                        Console.WriteLine($"Invalid duration. Must be between 12 and {maxDurationMonths} months. Please try again.");
                    else
                        Console.WriteLine($"Maximum attempts reached for duration. Investment cancelled.");
                }
            }
            return validDuration;
        }

        private void ProcessSipInvestment(decimal sipAmount, int durationInMonths, decimal pricePerUnit)
        {
            decimal expense = sipAmount * ExpenseRatio;
            decimal netAmount = sipAmount - expense;
            decimal unitsPerInstallment = Math.Round(netAmount / pricePerUnit, 2); // Units per installment
            decimal currentAmountPerInstallment = unitsPerInstallment * pricePerUnit;

            // Set SIP to start next month
            DateTime today = DateTime.Today;
            DateTime nextMonthStart = today.AddMonths(1).AddDays(-today.Day + 1); // First day of next month
            DateTime nextInstallmentDate = nextMonthStart.AddMonths(1);
            DateTime endDate = nextMonthStart.AddMonths(durationInMonths);

            _userSip.sipAmount = sipAmount;
            _userSip.totalUnits = 0m; // No units yet
            _userSip.totalInstallments = 0; // No installments yet
            _userSip.totalInvestedAmount = 0m; // No investment yet
            _userSip.currentAmount = 0m; // No current amount yet

            Console.WriteLine($"\nSIP Details:");
            Console.WriteLine($"Fund Name: {_userSip.fundName}");
            Console.WriteLine($"SIP Amount: ₹ {sipAmount}");
            Console.WriteLine($"Total Invested Amount: ₹ {_userSip.totalInvestedAmount}");
            Console.WriteLine($"Expense (1%): ₹ {expense}");
            Console.WriteLine($"Current Amount after expense: ₹ {_userSip.currentAmount}");
            Console.WriteLine($"Start Date: {nextMonthStart:yyyy-MM-dd}");
            Console.WriteLine($"Duration: {durationInMonths} months");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Next Installment Date: {nextInstallmentDate:yyyy-MM-dd}");

            Console.Write("Confirm SIP investment? (yes/no): ");
            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "yes")
            {
                _userSip.durationInMonths = durationInMonths;
                _userSip.sipStartDate = nextMonthStart; // Start next month
                _userSip.sipEndDate = endDate;
                _userSip.nextInstallmentDate = nextInstallmentDate;

                // No immediate deduction or investment; handled by IncrementInstallments
                _fundBussines.SaveSIPInvest();
            }
            else
                Console.WriteLine("Investment cancelled.");
        }
    }
}