using Microsoft.Extensions.DependencyInjection;
using MutualFundSimulatorService.Model;
using System;

namespace MutualFundSimulatorService.Business
{
    public class Lumpsum
    {
        private readonly MutualFundBusiness _fundBusiness;
        private readonly UserLumpsumInvest _userLumpsum;
        private readonly User _user;

        public Lumpsum(MutualFundBusiness fundBusiness, UserLumpsumInvest userLumpsum, User user)
        {
            _fundBusiness = fundBusiness;
            _userLumpsum = userLumpsum;
            _user = user;
        }

        /// <summary>
        /// Initiates the Lump Sum investment process for a specified fund, prompting for amount, duration, and start date.
        /// </summary>
        /// <param name="fundName"></param>
        public void LumpSumInvest(string fundName)
        {
            try
            {
                _userLumpsum.fundName = fundName;
                Console.WriteLine($"\n--- Lumpsum Investing in {_userLumpsum.fundName} ---");
                decimal pricePerUnit = _fundBusiness.GetFundPrice(_userLumpsum.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                decimal amount;
                if (!GetValidLumpSumAmount(pricePerUnit, out amount))
                {
                    return;
                }

                if (_user.walletBalance < amount)
                {
                    Console.WriteLine($"Insufficient funds in wallet (₹{_user.walletBalance}). Please add money to your wallet.");
                    return;
                }
                ProcessInvestment(pricePerUnit, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user to enter a valid lump sum amount (minimum ₹5000) and calculates investment details.
        /// </summary>
        /// <param name="pricePerUnit"></param>
        /// <param name="amount">Outputs the original amount entered by the user</param>
        /// <returns></returns>
        private bool GetValidLumpSumAmount(decimal pricePerUnit, out decimal amount)
        {
            int attempts = 0;
            bool validAmount = false;
            amount = 0m;

            while (attempts < 3 && !validAmount)
            {
                Console.Write($"Enter the lump sum amount you want to invest (minimum ₹5000): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal inputAmount) && inputAmount >= 5000)
                {
                    amount = inputAmount;
                    decimal monthlyExpenseRatio = GetExpenseRatio(_userLumpsum.fundName);
                    decimal expense = amount * monthlyExpenseRatio / 100m;
                    decimal netAmount = amount - expense;

                    _userLumpsum.investedAmount = netAmount;
                    _userLumpsum.quantity = Math.Round(netAmount / pricePerUnit, 2);
                    _userLumpsum.currentAmount = _userLumpsum.quantity * pricePerUnit;
                    _userLumpsum.lumpsumStartDate = User.CurrentDate;

                    validAmount = true;
                    Console.WriteLine($"Total units purchased: {_userLumpsum.quantity} units at ₹ {pricePerUnit} per unit.");
                    Console.WriteLine($"Monthly Expense Ratio: {monthlyExpenseRatio}% (₹ {expense:F2} deducted for first month)");
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

        /// <summary>
        /// Processes the lump sum investment, deducting funds and saving details if confirmed by the user.
        /// </summary>
        /// <param name="pricePerUnit"></param>
        /// <param name="originalAmount">The original amount entered by the user</param>
        private void ProcessInvestment(decimal pricePerUnit, decimal originalAmount)
        {
            Console.Write("Confirm lump sum investment? (yes/no): ");
            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "yes")
            {
                if (_user.walletBalance >= originalAmount)
                {
                    _fundBusiness.AddMoneyToWallet(-originalAmount);
                    _fundBusiness.SaveExpense(_userLumpsum.fundName, originalAmount - _userLumpsum.investedAmount, _userLumpsum.lumpsumStartDate);
                    _fundBusiness.SaveLumpsumInvest(true);
                    Console.WriteLine("Lump sum investment successful!");
                }
                else
                {
                    Console.WriteLine($"Insufficient funds for lump sum investment. Required: ₹{originalAmount:F2}, Available: ₹{_user.walletBalance}");
                }
            }
            else
            {
                Console.WriteLine("Investment cancelled.");
            }
        }
        public void ProcessInvestment(string fundName, decimal amount, bool deducted)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fundName))
                    throw new ArgumentException("Fund name is required.");
                if (amount < 5000)
                    throw new ArgumentException("Lump sum amount must be at least ₹5000.");

                _userLumpsum.fundName = fundName;
                decimal pricePerUnit = _fundBusiness.GetFundPrice(_userLumpsum.fundName);
                if (pricePerUnit <= 0)
                    throw new ArgumentException("Invalid fund price.");

                decimal monthlyExpenseRatio = GetExpenseRatio(_userLumpsum.fundName);
                decimal expense = amount * monthlyExpenseRatio / 100m;
                decimal netAmount = amount - expense;

                _userLumpsum.investedAmount = netAmount;
                _userLumpsum.quantity = Math.Round(netAmount / pricePerUnit, 2);
                _userLumpsum.currentAmount = _userLumpsum.quantity * pricePerUnit;
                _userLumpsum.lumpsumStartDate = User.CurrentDate;

                if (_user.walletBalance < amount)
                    throw new ArgumentException($"Insufficient funds in wallet (₹{_user.walletBalance}). Required: ₹{amount}");

                _fundBusiness.AddMoneyToWallet(-amount);
                _fundBusiness.SaveExpense(_userLumpsum.fundName, expense, _userLumpsum.lumpsumStartDate);
                _fundBusiness.SaveLumpsumInvest(deducted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to process lumpsum investment: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Retrieve the monthly expense ratio for a specific fund
        /// </summary>
        /// <param name="fundName"></param>
        /// <returns></returns>
        private decimal GetExpenseRatio(string fundName)
        {
            var fundDetails = MutualFundSimulatorUtility.GetFundDetails();
            return fundDetails.TryGetValue(fundName, out var details) ? details.expenseRatio : 0.070m;
        }
    }
}