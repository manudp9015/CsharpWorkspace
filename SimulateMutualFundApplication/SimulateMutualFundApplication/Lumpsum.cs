using MutualFundSimulatorApplication.Business;
using MutualFundSimulatorApplication.Model;
using System;

namespace MutualFundSimulatorApplication
{
    internal class Lumpsum
    {
        private MutualFundBusiness _fundBussines;
        private UserLumpsumInvest _userLumpsum;
        private const decimal ExpenseRatio = 0.01m; // 1% expense ratio
        private User _user;

        public Lumpsum(MutualFundBusiness fundBussines, UserLumpsumInvest userLumpsum, User user)
        {
            _fundBussines = fundBussines;
            _userLumpsum = userLumpsum;
            _user = user;
        }

        public void LumpSumInvest(string fundname)
        {
            try
            {
                _userLumpsum.fundName = fundname;
                Console.WriteLine($"\n--- Lumpsum Investing in {_userLumpsum.fundName} ---");
                decimal pricePerUnit = _fundBussines.GetFundPrice(_userLumpsum.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                if (!GetValidLumpSumAmount(out decimal lumpSumAmount, pricePerUnit))
                {
                    return;
                }

                // Check wallet balance with <=
                if (_user.walletBalance < lumpSumAmount)
                {
                    Console.WriteLine($"Insufficient funds in wallet (₹{_user.walletBalance}). Please add money to your wallet.");
                    return;
                }

                if (!GetValidDuration(out int durationInMonths))
                {
                    return;
                }

                ProcessInvestment(lumpSumAmount, durationInMonths, pricePerUnit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private bool GetValidLumpSumAmount(out decimal lumpSumAmount, decimal pricePerUnit)
        {
            int attempts = 0;
            lumpSumAmount = 0;
            bool validAmount = false;

            while (attempts < 3 && !validAmount)
            {
                Console.Write($"Enter the lump sum amount you want to invest (minimum ₹5000): ");
                if (decimal.TryParse(Console.ReadLine(), out lumpSumAmount) && lumpSumAmount >= 5000)
                {
                    validAmount = true;
                    decimal expense = lumpSumAmount * ExpenseRatio;
                    decimal netAmount = lumpSumAmount - expense;
                    _userLumpsum.quantity = Math.Round(netAmount / pricePerUnit, 2);
                    _userLumpsum.investedAmount = netAmount; // Changed to netAmount
                    _userLumpsum.currentAmount = _userLumpsum.quantity * pricePerUnit;
                    Console.WriteLine($"Total units purchased: {_userLumpsum.quantity} units at ₹ {pricePerUnit} per unit.");
                    Console.WriteLine($"Total lump sum amount: ₹ {lumpSumAmount}");
                    Console.WriteLine($"Expense (1%): ₹ {expense}");
                    Console.WriteLine($"Current amount after expense: ₹ {_userLumpsum.currentAmount}");
                }
                else
                {
                    attempts++;
                    if (attempts < 3)
                        Console.WriteLine("Invalid lump sum amount. Minimum amount is ₹5000. Please try again.");
                    else
                        Console.WriteLine($"Failed to enter a valid amount after 3 attempts. Investment cancelled.");
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
                Console.Write($"Enter the duration in months (minimum 12, maximum {maxDurationMonths}): ");
                if (int.TryParse(Console.ReadLine(), out durationInMonths) && durationInMonths >= 12 && durationInMonths <= maxDurationMonths)
                    validDuration = true;
                else
                {
                    attempts++;
                    if (attempts < 3)
                        Console.WriteLine($"Invalid duration. Must be between 12 and {maxDurationMonths} months. Please try again.");
                    else
                        Console.WriteLine($"Failed to enter a valid duration after 3 attempts. Investment cancelled.");
                }
            }
            return validDuration;
        }

        private void ProcessInvestment(decimal lumpSumAmount, int durationInMonths, decimal pricePerUnit)
        {
            Console.Write("Confirm lump sum investment? (yes/no): ");
            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "yes")
            {
                _userLumpsum.durationInMonths = durationInMonths;
                _userLumpsum.lumpsumStartDate = DateTime.Today;
                _userLumpsum.lumpsumEndDate = _userLumpsum.lumpsumStartDate.AddMonths(_userLumpsum.durationInMonths);

                // Deduct from wallet and persist to database in one step
                _fundBussines.AddMoneyToWallet(-lumpSumAmount);
                Console.WriteLine($"Deducted ₹{lumpSumAmount} from wallet. New wallet balance: ₹{_user.walletBalance}");

                decimal expense = lumpSumAmount * ExpenseRatio;
                _fundBussines.SaveExpense(_userLumpsum.fundName, expense, DateTime.Today);
                _fundBussines.SaveLumpsumInvest();
            }
            else
                Console.WriteLine("Investment cancelled.");
        }
    }
}