using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class Lumpsum
    {
        private MutualFundBusiness _fundBussines;
        private UserLumpsumInvest _userLumpsum;

        public Lumpsum(MutualFundBusiness fundBussines, UserLumpsumInvest userLumpsum)
        {
            _fundBussines = fundBussines;
            _userLumpsum = userLumpsum;
        }

        public void LumpSumInvest(string fundname)
        {
            try
            {
                _userLumpsum.fundName = fundname;
                Console.WriteLine($"\n--- Investing in {_userLumpsum.fundName} ---");

                decimal pricePerUnit = _fundBussines.GetFundPrice(_userLumpsum.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                if (!GetValidLumpSumAmount(out decimal lumpSumAmount, pricePerUnit))
                {
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
            int maxAttempts = 3;
            int attempts = 0;
            lumpSumAmount = 0;
            bool validAmount = false;

            while (attempts < maxAttempts && !validAmount)
            {
                Console.Write($"Enter the lump sum amount you want to invest (minimum ₹5000): ");
                if (decimal.TryParse(Console.ReadLine(), out lumpSumAmount) && lumpSumAmount >= 5000)
                {
                    validAmount = true;
                    _userLumpsum.quantity = Math.Round(lumpSumAmount / pricePerUnit, 2);
                    Console.WriteLine($"Total units purchased: {_userLumpsum.quantity} units at ₹ {pricePerUnit} per unit.");
                    Console.WriteLine($"Total investment amount: ₹ {lumpSumAmount}");
                    _userLumpsum.investedAmount = lumpSumAmount;
                }
                else
                {
                    attempts++;
                    if (attempts < maxAttempts)
                    {
                        Console.WriteLine("Invalid lump sum amount. Minimum amount is ₹5000. Please try again.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to enter a valid amount after {maxAttempts} attempts. Investment cancelled.");
                    }
                }
            }
            return validAmount;
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
                Console.Write($"Enter the duration in months (minimum 12, maximum {maxDurationMonths}): ");
                if (int.TryParse(Console.ReadLine(), out durationInMonths) && durationInMonths >= 12 && durationInMonths <= maxDurationMonths)
                    validDuration = true;
                else
                {
                    attempts++;
                    if (attempts < maxAttempts)
                        Console.WriteLine($"Invalid duration. Must be between 12 and {maxDurationMonths} months. Please try again.");
                    else
                        Console.WriteLine($"Failed to enter a valid duration after {maxAttempts} attempts. Investment cancelled.");
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
                _fundBussines.SaveLumpsumInvest();
            }
            else
                Console.WriteLine("Investment cancelled.");
        }
    }
}