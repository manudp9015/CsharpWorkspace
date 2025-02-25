using MutualFundSimulatorApplication.Business;
using MutualFundSimulatorApplication.Model;
using System;

namespace MutualFundSimulatorApplication
{
    internal class Sip
    {
        private MutualFundBusiness _fundBussines;
        private UserSipInvest _userSip;
        private User _user;

        public Sip(MutualFundBusiness fundBussines, UserSipInvest userSip, User user)
        {
            _fundBussines = fundBussines;
            _userSip = userSip;
            _user = user;
        }

        /// <summary>
        /// Initiates the SIP investment process for a specified fund, amount, start date, and duration.
        /// </summary>
        /// <param name="fundName"></param>
        public void SIPInvest(string fundName)
        {
            try
            {
                _userSip.fundName = fundName;
                Console.WriteLine($"\n--- Sip Investing in {_userSip.fundName} ---");
                decimal pricePerUnit = _fundBussines.GetFundPrice(_userSip.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                if (!GetValidSipAmount())
                    return;
                if (_user.walletBalance < _userSip.sipAmount)
                {
                    Console.WriteLine($"Insufficient funds in wallet (₹{_user.walletBalance}). Please add money to your wallet.");
                    return;
                }
                if (!GetValidStartDate())
                    return;
                if (!GetValidDuration())
                    return;
                ProcessSipInvestment(pricePerUnit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// This method ensure the user to enter a valid SIP amount (minimum ₹500).
        /// </summary>
        /// <returns></returns>
        private bool GetValidSipAmount()
        {
            int attempts = 0;
            bool validAmount = false;
            while (attempts < 3 && !validAmount)
            {
                Console.Write($"Enter SIP amount (minimum ₹500): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount >= 500)
                {
                    _userSip.sipAmount = amount;
                    validAmount = true;
                    decimal monthlyExpenseRatio = GetExpenseRatio(_userSip.fundName);
                    Console.WriteLine($"SIP amount: ₹ {_userSip.sipAmount}");
                    Console.WriteLine($"Monthly Expense Ratio: {monthlyExpenseRatio}%");
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

        /// <summary>
        /// This method ensure the user to enter a valid SIP start date (today or later).
        /// </summary>
        /// <returns></returns>
        private bool GetValidStartDate()
        {
            int attempts = 0;
            bool validDate = false;

            while (attempts < 3 && !validDate)
            {
                Console.Write($"Enter SIP start date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate) && startDate >= User.CurrentDate)
                {
                    _userSip.sipStartDate = startDate;
                    validDate = true;
                }
                else
                {
                    attempts++;
                    if (attempts < 3)
                        Console.WriteLine($"Invalid date. Must be {User.CurrentDate:yyyy-MM-dd} or later (yyyy-MM-dd). Please try again.");
                    else
                        Console.WriteLine($"Maximum attempts reached for date. Investment cancelled.");
                }
            }
            return validDate;
        }

        /// <summary>
        /// This method ensure the user to enter a valid SIP duration (1 to 1200 months).
        /// </summary>
        /// <returns></returns>
        private bool GetValidDuration()
        {
            int attempts = 0;
            bool validDuration = false;
            const int maxDurationMonths = 1200;
            while (attempts < 3 && !validDuration)
            {
                Console.Write($"Enter SIP duration in months (minimum 12, maximum {maxDurationMonths}): ");
                if (int.TryParse(Console.ReadLine(), out int months) && months >= 12 && months <= maxDurationMonths)
                {
                    _userSip.durationInMonths = months;
                    validDuration = true;
                }
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

        /// <summary>
        /// Displays SIP investment details and saves the investment if confirmed by the user.
        /// </summary>
        /// <param name="pricePerUnit"></param>
        private void ProcessSipInvestment(decimal pricePerUnit)
        {
            _userSip.nextInstallmentDate = _userSip.sipStartDate.AddMonths(1);
            _userSip.sipEndDate = _userSip.sipStartDate.AddMonths(_userSip.durationInMonths);
            _userSip.totalUnits = 0m;
            _userSip.totalInstallments = 0;
            _userSip.totalInvestedAmount = 0m;
            _userSip.currentAmount = 0m;

            decimal monthlyExpenseRatio = GetExpenseRatio(_userSip.fundName);
            decimal monthlyExpense = _userSip.sipAmount * monthlyExpenseRatio / 100m;

            Console.WriteLine($"\nSIP Details:");
            Console.WriteLine($"Fund Name: {_userSip.fundName}");
            Console.WriteLine($"SIP Amount: ₹ {_userSip.sipAmount}");
            Console.WriteLine($"Total Invested Amount: ₹ {_userSip.totalInvestedAmount}");
            Console.WriteLine($"Monthly Expense: ₹ {monthlyExpense:F2} (based on {monthlyExpenseRatio}% monthly ratio)");
            Console.WriteLine($"Current Amount: ₹ {_userSip.currentAmount}");
            Console.WriteLine($"Start Date: {_userSip.sipStartDate:yyyy-MM-dd}");
            Console.WriteLine($"Duration: {_userSip.durationInMonths} months");
            Console.WriteLine($"End Date: {_userSip.sipEndDate:yyyy-MM-dd}");
            Console.WriteLine($"Next Installment Date: {_userSip.nextInstallmentDate:yyyy-MM-dd}");

            Console.Write("Confirm SIP investment? (yes/no): ");
            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "yes")
                _fundBussines.SaveSIPInvest();
            else
                Console.WriteLine("Investment cancelled.");
        }

        /// <summary>
        /// Retrieves the expense ratio for the specified fund from fund details.
        /// </summary>
        /// <param name="fundName"></param>
        /// <returns></returns>
        private decimal GetExpenseRatio(string fundName)
        {
            var fundDetails = new MutualFundSimulatorUtility(null, null, null, null, null).GetFundDetails();
            return fundDetails.TryGetValue(fundName, out var details) ? details.expenseRatio : 0.070m;
        }
    }
}